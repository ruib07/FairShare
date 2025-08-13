using FairShare.Application.Interfaces.Repositories;
using FairShare.Application.Interfaces.Services;

namespace FairShare.Infrastructure.Services;

public class GroupMembersService : IGroupMembersService
{
    private readonly IGroupMemberRepository _groupMemberRepository;

    public GroupMembersService(IGroupMemberRepository groupMemberRepository)
    {
        _groupMemberRepository = groupMemberRepository ?? 
            throw new ArgumentNullException(nameof(groupMemberRepository));
    }

    public async Task<IEnumerable<Guid>> GetGroupsForUser(Guid userId)
    {
        return await _groupMemberRepository.GetGroupsForUser(userId);
    }

    public async Task<IEnumerable<Guid>> GetUsersInGroup(Guid groupId)
    {
        return await _groupMemberRepository.GetUsersInGroup(groupId);
    }

    public async Task<bool> IsUserInGroup(Guid groupId, Guid userId)
    {
        return await _groupMemberRepository.IsUserInGroup(groupId, userId);
    }

    public async Task AddUserToGroup(Guid groupId, Guid userId)
    {
        await _groupMemberRepository.AddUserToGroup(groupId, userId);
    }

    public async Task RemoveUserFromGroup(Guid groupId, Guid userId)
    {
        await _groupMemberRepository.RemoveUserFromGroup(groupId, userId);
    }
}
