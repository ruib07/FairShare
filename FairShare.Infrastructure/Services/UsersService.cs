using FairShare.Application.Helpers;
using FairShare.Application.Interfaces.Repositories;
using FairShare.Application.Interfaces.Services;
using FairShare.Application.Shared.Common;
using FairShare.Domain.Models;

namespace FairShare.Infrastructure.Services;

public class UsersService : IUsersService
{
    private readonly IUserRepository _userRepository;

    public UsersService(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? 
            throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<Result<User>> GetUserById(Guid userId)
    {
        var user = await _userRepository.GetUserById(userId);

        if (user == null) return Result<User>.Fail("User not found.", 404);

        return Result<User>.Success(user);
    }

    public async Task<Result<User>> CreateUser(User user)
    {
        var validation = ValidateUserFields(user);
        
        if (!validation.IsSuccess) 
            return Result<User>.Fail(validation.Error.Message, validation.Error.StatusCode);

        var emailCheck = await ValidateExistingEmail(user);

        if (!emailCheck.IsSuccess) 
            return Result<User>.Fail(emailCheck.Error.Message, emailCheck.Error.StatusCode);

        user.PasswordHash = EncryptionHelper.HashPassword(user.PasswordHash);

        var createdUser = await _userRepository.CreateUser(user);

        return Result<User>.Success(createdUser, "User created successfully.");
    }

    public async Task<Result<User>> UpdateUser(Guid userId, User updateUser)
    {
        var currentUser = await _userRepository.GetUserById(userId);

        var skipPassword = string.IsNullOrWhiteSpace(updateUser.PasswordHash);
        var validation = ValidateUserFields(updateUser, skipPassword);

        if (!validation.IsSuccess) 
            return Result<User>.Fail(validation.Error.Message, validation.Error.StatusCode);

        var emailCheck = await ValidateExistingEmail(updateUser, userId);

        if (!emailCheck.IsSuccess) 
            return Result<User>.Fail(emailCheck.Error.Message, emailCheck.Error.StatusCode);

        currentUser.Name = updateUser.Name;
        currentUser.Email = updateUser.Email;
        if (!skipPassword)
        {
            currentUser.PasswordHash = EncryptionHelper.HashPassword(updateUser.PasswordHash);
        }

        await _userRepository.UpdateUser(currentUser);

        return Result<User>.Success(currentUser, "User updated successfully.");
    }

    public async Task DeleteUser(Guid userId)
    {
        await _userRepository.DeleteUser(userId);
    }

    #region Private Methods

    private static Result<bool> ValidateUserFields(User user, bool skipPassword = false)
    {
        if (string.IsNullOrWhiteSpace(user.Name))
            return Result<bool>.Fail("Name is required.", 400);

        if (string.IsNullOrWhiteSpace(user.Email) || !user.Email.Contains('@'))
            return Result<bool>.Fail("Valid email is required.", 400);

        if (!skipPassword)
        {
            if (!PasswordPolicyHelper.IsValid(user.PasswordHash))
                return Result<bool>.Fail("Password must be at least 8 characters long and contain at least one uppercase letter, " +
                                            "one lowercase letter, one number, and one special character.", 400);
        }

        return Result<bool>.Success(true);
    }

    private async Task<Result<bool>> ValidateExistingEmail(User user, Guid? currentUserId = null)
    {
        var existing = await _userRepository.GetUserByEmail(user.Email);

        if (existing != null && existing.Id != currentUserId)
            return Result<bool>.Fail("User with the same email already exists.", 409);

        return Result<bool>.Success(true);
    }

    #endregion Private Methods
}
