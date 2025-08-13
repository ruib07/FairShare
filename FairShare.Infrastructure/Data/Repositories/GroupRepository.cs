using FairShare.Application.Interfaces.Repositories;
using FairShare.Domain.Models;
using FairShare.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FairShare.Infrastructure.Data.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly FairShareDbContext _context;
    private DbSet<Group> Groups => _context.Groups;

    public GroupRepository(FairShareDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Group> GetGroupById(Guid groupId)
    {
        return await Groups.FirstOrDefaultAsync(g => g.Id == groupId);
    }

    public async Task<IEnumerable<Group>> GetGroupsCreatedByUserId(Guid userId)
    {
        return await Groups.AsNoTracking().Where(g => g.CreatedByUserId == userId).ToListAsync();
    }

    public async Task<Group> CreateGroup(Group group)
    {
        await Groups.AddAsync(group);
        await _context.SaveChangesAsync();

        return group;
    }

    public async Task UpdateGroup(Group group)
    {
        Groups.Update(group);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteGroup(Guid groupId)
    {
        var group = await GetGroupById(groupId);

        Groups.Remove(group);
        await _context.SaveChangesAsync();
    }
}
