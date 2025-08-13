using FairShare.Application.Interfaces.Repositories;
using FairShare.Domain.Models;
using FairShare.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FairShare.Infrastructure.Data.Repositories;

public class SettlementRepository : ISettlementRepository
{
    private readonly FairShareDbContext _context;
    private DbSet<Settlement> Settlements => _context.Settlements;

    public SettlementRepository(FairShareDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Settlement> GetSettlementById(Guid settlementId)
    {
        return await Settlements.FirstOrDefaultAsync(s => s.Id == settlementId);
    }

    public async Task<IEnumerable<Settlement>> GetSettlementsByGroupId(Guid groupId)
    {
        return await Settlements.AsNoTracking().Where(s => s.GroupId == groupId).ToListAsync();
    }

    public async Task<IEnumerable<Settlement>> GetSettlementsFromUserId(Guid userId)
    {
        return await Settlements.AsNoTracking().Where(s => s.FromUserId == userId).ToListAsync();
    }

    public async Task<IEnumerable<Settlement>> GetSettlementsToUserId(Guid userId)
    {
        return await Settlements.AsNoTracking().Where(s => s.ToUserId == userId).ToListAsync();
    }

    public async Task<Settlement> CreateSettlement(Settlement settlement)
    {
        await Settlements.AddAsync(settlement);
        await _context.SaveChangesAsync();

        return settlement;
    }

    public async Task UpdateSettlement(Settlement settlement)
    {
        Settlements.Update(settlement);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSettlement(Guid settlementId)
    {
        var settlement = await GetSettlementById(settlementId);

        Settlements.Remove(settlement);
        await _context.SaveChangesAsync();
    }
}
