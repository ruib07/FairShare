using FairShare.Domain.Models;

namespace FairShare.APIClient.Contracts;

public interface IExpensesApiService
{
    Task<Expense> GetExpenseById(Guid expenseId);
    Task<IEnumerable<Expense>> GetExpensesByGroupId(Guid groupId);
    Task<IEnumerable<Expense>> GetPaidExpensesByUserId(Guid userId);
    Task<Expense> CreateExpense(Expense expense);
    Task UpdateExpense(Expense expense);
    Task DeleteExpense(Guid expenseId);
}
