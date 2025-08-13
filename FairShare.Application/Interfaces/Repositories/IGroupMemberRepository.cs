namespace FairShare.Application.Interfaces.Repositories;

public interface IGroupMemberRepository
{
    Task<IEnumerable<Guid>> GetUsersInGroup(Guid groupId);
    Task<IEnumerable<Guid>> GetGroupsForUser(Guid userId);
    Task<bool> IsUserInGroup(Guid groupId, Guid userId);
    Task AddUserToGroup(Guid groupId, Guid userId);
    Task RemoveUserFromGroup(Guid groupId, Guid userId);
}
