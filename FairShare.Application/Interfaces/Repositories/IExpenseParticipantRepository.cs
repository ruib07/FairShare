using FairShare.Domain.Models;

namespace FairShare.Application.Interfaces.Repositories;

public interface IExpenseParticipantRepository
{
    Task<IEnumerable<Guid>> GetParticipantsByExpenseId(Guid expenseId);
    Task<bool> IsUserParticipating(Guid expenseId, Guid userId);
    Task AddParticipant(ExpenseParticipant expenseParticipant);
    Task RemoveParticipant(Guid expenseId, Guid userId);
}
