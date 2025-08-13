using FairShare.Application.Shared.Common;
using FairShare.Domain.Models;

namespace FairShare.Application.Interfaces.Services;

public interface IUsersService
{
    Task<Result<User>> GetUserById(Guid userId);
    Task<Result<User>> CreateUser(User user);
    Task<Result<User>> UpdateUser(Guid userId, User updateUser); 
    Task DeleteUser(Guid userId);
}
