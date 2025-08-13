using FairShare.Application.Interfaces.Repositories;
using FairShare.Domain.Enums;
using FairShare.Domain.Models;
using FairShare.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FairShare.Infrastructure.Data.Repositories;

public class GroupMemberRepository : IGroupMemberRepository
{
    private readonly FairShareDbContext _context;
    private DbSet<GroupMember> GroupMembers => _context.GroupMembers;

    public GroupMemberRepository(FairShareDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Guid>> GetGroupsForUser(Guid userId)
    {
        return await GroupMembers.AsNoTracking()
                                 .Where(gm => gm.UserId == userId)
                                 .Select(gm => gm.GroupId)
                                 .ToListAsync();
    }

    public async Task<IEnumerable<Guid>> GetUsersInGroup(Guid groupId)
    {
        return await GroupMembers.AsNoTracking()
                                 .Where(gm => gm.GroupId == groupId)
                                 .Select(gm => gm.UserId)
                                 .ToListAsync();
    }

    public async Task<bool> IsUserInGroup(Guid groupId, Guid userId)
    {
        return await GroupMembers.AnyAsync(gm => gm.GroupId == groupId && gm.UserId == userId);
    }

    public async Task AddUserToGroup(Guid groupId, Guid userId)
    {
        var exists = await GroupMembers.AnyAsync(gm => gm.GroupId == groupId && gm.UserId == userId);
        if (exists) return;

        await GroupMembers.AddAsync(new GroupMember()
        {
            GroupId = groupId,
            UserId = userId,
            JoinedAt = DateTime.Now,
            Role = MemberRoles.Member
        });

        await _context.SaveChangesAsync();
    }

    public async Task RemoveUserFromGroup(Guid groupId, Guid userId)
    {
        var groupMember = await GroupMembers.FirstOrDefaultAsync(gm => gm.GroupId == groupId && gm.UserId == userId);

        if (groupMember == null) return;

        GroupMembers.Remove(groupMember);
        await _context.SaveChangesAsync();
    }
}
