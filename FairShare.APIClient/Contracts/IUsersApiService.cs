using FairShare.Domain.Models;

namespace FairShare.APIClient.Contracts;

public interface IUsersApiService
{
    Task<User> GetUserById(Guid userId);
    Task UpdateUser(User user);
    Task DeleteUser(Guid userId);
}
