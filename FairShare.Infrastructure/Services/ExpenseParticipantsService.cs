using FairShare.Application.Interfaces.Repositories;
using FairShare.Application.Interfaces.Services;
using FairShare.Application.Shared.Common;
using FairShare.Domain.Models;

namespace FairShare.Infrastructure.Services;

public class ExpenseParticipantsService : IExpenseParticipantsService
{
    private readonly IExpenseParticipantRepository _expenseParticipantRepository;

    public ExpenseParticipantsService(IExpenseParticipantRepository expenseParticipantRepository)
    {
        _expenseParticipantRepository = expenseParticipantRepository ?? 
            throw new ArgumentNullException(nameof(expenseParticipantRepository));
    }

    public async Task<IEnumerable<Guid>> GetParticipantsByExpenseId(Guid expenseId)
    {
        return await _expenseParticipantRepository.GetParticipantsByExpenseId(expenseId);
    }

    public async Task<bool> IsUserParticipating(Guid expenseId, Guid userId)
    {
        return await _expenseParticipantRepository.IsUserParticipating(expenseId, userId);
    }

    public async Task AddParticipant(ExpenseParticipant expenseParticipant)
    {
        var validation = ValidateExpenseParticipantFields(expenseParticipant);

        if (!validation.IsSuccess) return;

        await _expenseParticipantRepository.AddParticipant(expenseParticipant);
    }

    public async Task RemoveParticipant(Guid expenseId, Guid userId)
    {
        await _expenseParticipantRepository.RemoveParticipant(expenseId, userId);
    }

    #region Private Methods

    private static Result<bool> ValidateExpenseParticipantFields(ExpenseParticipant expenseParticipant)
    {
        if (expenseParticipant.ShareAmount <= 0)
            return Result<bool>.Fail("Shared amount must be greater than zero.", 400);

        return Result<bool>.Success(true);
    }

    #endregion Private Methods
}
