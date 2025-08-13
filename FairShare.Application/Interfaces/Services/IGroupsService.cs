using FairShare.Application.Shared.Common;
using FairShare.Domain.Models;

namespace FairShare.Application.Interfaces.Services;

public interface IGroupsService
{
    Task<Result<Group>> GetGroupById(Guid groupId);
    Task<IEnumerable<Group>> GetGroupsCreatedByUserId(Guid userId);
    Task<Result<Group>> CreateGroup(Group group);
    Task<Result<Group>> UpdateGroup(Guid groupId, Group updateGroup);
    Task DeleteGroup(Guid groupId);
}
