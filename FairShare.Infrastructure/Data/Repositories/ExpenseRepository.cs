using FairShare.Application.Interfaces.Repositories;
using FairShare.Domain.Models;
using FairShare.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FairShare.Infrastructure.Data.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly FairShareDbContext _context;
    private DbSet<Expense> Expenses => _context.Expenses;

    public ExpenseRepository(FairShareDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Expense> GetExpenseById(Guid expenseId)
    {
        return await Expenses.FirstOrDefaultAsync(e => e.Id == expenseId);
    }

    public async Task<IEnumerable<Expense>> GetExpensesByGroupId(Guid groupId)
    {
        return await Expenses.AsNoTracking().Where(e => e.GroupId == groupId).ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetPaidExpensesByUserId(Guid userId)
    {
        return await Expenses.AsNoTracking().Where(e => e.PaidByUserId == userId).ToListAsync();
    }

    public async Task<Expense> CreateExpense(Expense expense)
    {
        await Expenses.AddAsync(expense);
        await _context.SaveChangesAsync();

        return expense;
    }

    public async Task UpdateExpense(Expense expense)
    {
        Expenses.Update(expense);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteExpense(Guid expenseId)
    {
        var expense = await GetExpenseById(expenseId);

        Expenses.Remove(expense);
        await _context.SaveChangesAsync();
    }
}
