using FairShare.Application.Shared.DTOs;

namespace FairShare.APIClient.Contracts;

public interface IAuthenticationsApiService
{
    Task SignUp(AuthenticationsDTO.SignUp.Request signup);
    Task<AuthenticationsDTO.SignIn.Response> SignIn(AuthenticationsDTO.SignIn.Request signin);
    Task<AuthenticationsDTO.SignIn.Response> RefreshToken(AuthenticationsDTO.SignIn.RefreshTokenRequest request);
    Task LogOut(AuthenticationsDTO.SignIn.RefreshTokenRequest request);
}
