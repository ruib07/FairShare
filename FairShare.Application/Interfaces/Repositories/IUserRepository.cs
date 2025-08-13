using FairShare.Domain.Models;

namespace FairShare.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User> GetUserById(Guid userId);
    Task<User> GetUserByEmail(string email);
    Task<User> CreateUser(User user);
    Task UpdateUser(User user);
    Task DeleteUser(Guid userId);
}
