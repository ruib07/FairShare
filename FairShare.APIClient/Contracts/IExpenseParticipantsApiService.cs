using FairShare.Domain.Models;

namespace FairShare.APIClient.Contracts;

public interface IExpenseParticipantsApiService
{
    Task<IEnumerable<Guid>> GetParticipantsByExpenseId(Guid expenseId);
    Task<bool> IsUserParticipating(Guid expenseId, Guid userId);
    Task AddParticipant(ExpenseParticipant expenseParticipant);
    Task RemoveParticipant(Guid expenseId, Guid userId);
}
