using FairShare.Domain.Models;

namespace FairShare.Application.Interfaces.Repositories;

public interface IGroupRepository
{
    Task<Group> GetGroupById(Guid groupId);
    Task<IEnumerable<Group>> GetGroupsCreatedByUserId(Guid userId);
    Task<Group> CreateGroup(Group group);
    Task UpdateGroup(Group group);
    Task DeleteGroup(Guid groupId);
}
