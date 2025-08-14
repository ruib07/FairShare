using FairShare.Application.Constants;
using FairShare.Application.Helpers;
using FairShare.Application.Interfaces.Services;
using FairShare.Application.Shared.DTOs;
using FairShare.Domain.Models;
using FairShare.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FairShare.API.Controllers;

[Route($"api/{AppSettings.ApiVersion}/authentications")]
public class AuthenticationsController : ControllerBase
{
    private readonly FairShareDbContext _context;
    private readonly IUsersService _usersService;
    private readonly JwtDTO _jwtDTO;

    public AuthenticationsController(FairShareDbContext context, IUsersService usersService, JwtDTO jwtDTO)
    {
        _context = context;
        _usersService = usersService;
        _jwtDTO = jwtDTO;
    }

    // POST api/v1/authentications/signup
    [AllowAnonymous]
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody, Required] AuthenticationsDTO.SignUp.Request request)
    {
        if (request == null) return BadRequest("All fields are required.");

        if (string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password)) return BadRequest("Invalid input.");

        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (existingUser != null) return Conflict("Email already in use.");

        var newUser = new User()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            PasswordHash = request.Password,
            ProfilePicture = request.ProfilePicture
        };

        var createdUser = await _usersService.CreateUser(newUser);
        var response = new ResponsesDTO.Creation(createdUser.Message, createdUser.Data.Id);

        return CreatedAtAction(nameof(SignUp), new { userId = createdUser.Data.Id }, response);
    }

    // POST api/v1/authentications/signin
    [AllowAnonymous]
    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] AuthenticationsDTO.SignIn.Request signinRequest)
    {
        if (signinRequest == null) return BadRequest("Email and password are mandatory.");
        if (string.IsNullOrWhiteSpace(signinRequest.Email)) return BadRequest("Email is required.");
        if (string.IsNullOrWhiteSpace(signinRequest.Password)) return BadRequest("Password is required.");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == signinRequest.Email);

        if (user == null) return Unauthorized("User not found.");

        if (!EncryptionHelper.VerifyPassword(signinRequest.Password, user.PasswordHash))
            return Unauthorized("Incorrect password.");

        var oldTokens = await _context.RefreshTokens
                                      .Where(ot => ot.UserId == user.Id && !ot.IsRevoked && ot.ExpiresAt > DateTime.UtcNow)
                                      .ToListAsync();

        foreach (var token in oldTokens)
        {
            token.IsRevoked = true;
        }

        var (Token, ExpiresAt) = GenerateAccessToken(user);
        var refreshToken = new RefreshToken()
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = false
        };

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        return Ok(new AuthenticationsDTO.SignIn.Response(Token, refreshToken.Token, AppSettings.TokenType, ExpiresAt));
    }

    // POST api/v1/authentications/refresh
    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] AuthenticationsDTO.SignIn.RefreshTokenRequest request)
    {
        var storedToken = await _context.RefreshTokens
                                        .Include(rt => rt.User)
                                        .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken);

        if (request == null || string.IsNullOrWhiteSpace(request.RefreshToken))
            return BadRequest("Refresh token is required.");

        if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.UtcNow)
            return Unauthorized("Invalid or expired refresh token.");

        storedToken.IsRevoked = true;

        var user = storedToken.User;

        var newRefreshToken = new RefreshToken()
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = false
        };

        var (Token, ExpiresAt) = GenerateAccessToken(user);

        _context.RefreshTokens.Add(newRefreshToken);
        await _context.SaveChangesAsync();

        return Ok(new AuthenticationsDTO.SignIn.Response(Token, newRefreshToken.Token, AppSettings.TokenType, ExpiresAt));
    }

    // POST api/v1/authentications/logout
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] AuthenticationsDTO.SignIn.RefreshTokenRequest request)
    {
        var token = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == request.RefreshToken);

        if (token == null) return NotFound("Token not found.");

        token.IsRevoked = true;
        await _context.SaveChangesAsync();

        return Ok("Logged out.");
    }

    #region Private Methods

    private (string Token, DateTime ExpiresAt) GenerateAccessToken(User user)
    {
        var claims = new List<Claim>()
        {
            new(AppSettings.ClaimId, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Name),
            new(AppSettings.ClaimEmail, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtDTO.Key));
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _jwtDTO.Issuer,
            Audience = _jwtDTO.Audience,
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return (tokenHandler.WriteToken(token), tokenDescriptor.Expires!.Value);
    }

    #endregion
}
