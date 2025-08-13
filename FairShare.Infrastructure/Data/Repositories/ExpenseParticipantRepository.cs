using FairShare.Application.Interfaces.Repositories;
using FairShare.Domain.Models;
using FairShare.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FairShare.Infrastructure.Data.Repositories;

public class ExpenseParticipantRepository : IExpenseParticipantRepository
{
    private readonly FairShareDbContext _context;
    private DbSet<ExpenseParticipant> ExpenseParticipants => _context.ExpenseParticipants;

    public ExpenseParticipantRepository(FairShareDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Guid>> GetParticipantsByExpenseId(Guid expenseId)
    {
        return await ExpenseParticipants.AsNoTracking()
                                        .Where(ep => ep.ExpenseId == expenseId)
                                        .Select(ep => ep.UserId)
                                        .ToListAsync();
    }

    public async Task<bool> IsUserParticipating(Guid expenseId, Guid userId)
    {
        return await ExpenseParticipants.AnyAsync(ep => ep.ExpenseId == expenseId && ep.UserId == userId);
    }

    public async Task AddParticipant(ExpenseParticipant expenseParticipant)
    {
        await ExpenseParticipants.AddAsync(expenseParticipant);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveParticipant(Guid expenseId, Guid userId)
    {
        var participant = await ExpenseParticipants.FirstOrDefaultAsync(ep => ep.ExpenseId == expenseId && ep.UserId == userId);

        if (participant == null) return;

        ExpenseParticipants.Remove(participant);
        await _context.SaveChangesAsync();
    }
}
