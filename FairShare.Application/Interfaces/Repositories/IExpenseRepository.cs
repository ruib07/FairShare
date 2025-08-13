using FairShare.Domain.Models;

namespace FairShare.Application.Interfaces.Repositories;

public interface IExpenseRepository
{
    Task<Expense> GetExpenseById(Guid expenseId);
    Task<IEnumerable<Expense>> GetExpensesByGroupId(Guid groupId);
    Task<IEnumerable<Expense>> GetPaidExpensesByUserId(Guid userId);
    Task<Expense> CreateExpense(Expense expense);
    Task UpdateExpense(Expense expense);
    Task DeleteExpense(Guid expenseId);
}
