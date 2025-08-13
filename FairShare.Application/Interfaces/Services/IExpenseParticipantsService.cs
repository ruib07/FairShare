using FairShare.Application.Shared.Common;
using FairShare.Domain.Models;

namespace FairShare.Application.Interfaces.Services;

public interface IExpenseParticipantsService
{
    Task<IEnumerable<Guid>> GetParticipantsByExpenseId(Guid expenseId);
    Task<bool> IsUserParticipating(Guid expenseId, Guid userId);
    Task AddParticipant(ExpenseParticipant expenseParticipant);
    Task RemoveParticipant(Guid expenseId, Guid userId);
}
