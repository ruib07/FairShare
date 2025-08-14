using FairShare.Domain.Models;

namespace FairShare.APIClient.Contracts;

public interface IGroupsApiService
{
    Task<Group> GetGroupById(Guid groupId);
    Task<IEnumerable<Group>> GetGroupsCreatedByUserId(Guid userId);
    Task<Group> CreateGroup(Group group);
    Task UpdateGroup(Group group);
    Task DeleteGroup(Guid groupId);
}
