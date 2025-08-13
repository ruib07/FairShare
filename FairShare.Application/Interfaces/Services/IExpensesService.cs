using FairShare.Application.Shared.Common;
using FairShare.Domain.Models;

namespace FairShare.Application.Interfaces.Services;

public interface IExpensesService
{
    Task<Result<Expense>> GetExpenseById(Guid expenseId);
    Task<IEnumerable<Expense>> GetExpensesByGroupId(Guid groupId);
    Task<IEnumerable<Expense>> GetPaidExpensesByUserId(Guid userId);
    Task<Result<Expense>> CreateExpense(Expense expense);
    Task<Result<Expense>> UpdateExpense(Guid expenseId, Expense updateExpense);
    Task DeleteExpense(Guid expenseId);
}
