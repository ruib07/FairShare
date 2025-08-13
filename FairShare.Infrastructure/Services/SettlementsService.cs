using FairShare.Application.Interfaces.Repositories;
using FairShare.Application.Interfaces.Services;
using FairShare.Application.Shared.Common;
using FairShare.Domain.Models;

namespace FairShare.Infrastructure.Services;

public class SettlementsService : ISettlementsService
{
    private readonly ISettlementRepository _settlementRepository;

    public SettlementsService(ISettlementRepository settlementRepository)
    {
        _settlementRepository = settlementRepository ?? 
            throw new ArgumentNullException(nameof(settlementRepository));
    }

    public async Task<Result<Settlement>> GetSettlementById(Guid settlementId)
    {
        var settlement = await _settlementRepository.GetSettlementById(settlementId);

        if (settlement == null) return Result<Settlement>.Fail("Settlement not found.", 404);

        return Result<Settlement>.Success(settlement);
    }

    public async Task<IEnumerable<Settlement>> GetSettlementsByGroupId(Guid groupId)
    {
        return await _settlementRepository.GetSettlementsByGroupId(groupId);
    }

    public async Task<IEnumerable<Settlement>> GetSettlementsFromUserId(Guid userId)
    {
        return await _settlementRepository.GetSettlementsFromUserId(userId);
    }

    public async Task<IEnumerable<Settlement>> GetSettlementsToUserId(Guid userId)
    {
        return await _settlementRepository.GetSettlementsToUserId(userId);
    }

    public async Task<Result<Settlement>> CreateSettlement(Settlement settlement)
    {
        var validation = ValidateSettlementFields(settlement);

        if (!validation.IsSuccess) 
            return Result<Settlement>.Fail(validation.Error.Message, validation.Error.StatusCode);

        var createdSettlement = await _settlementRepository.CreateSettlement(settlement);

        return Result<Settlement>.Success(createdSettlement, "Settlement created successfully.");
    }

    public async Task<Result<Settlement>> UpdateSettlement(Guid settlementId, Settlement updateSettlement)
    {
        var currentSettlement = await _settlementRepository.GetSettlementById(settlementId);

        var validation = ValidateSettlementFields(updateSettlement);

        if (!validation.IsSuccess) 
            return Result<Settlement>.Fail(validation.Error.Message, validation.Error.StatusCode);

        currentSettlement.Amount = updateSettlement.Amount;
        currentSettlement.Date = updateSettlement.Date;
        currentSettlement.ToUser = updateSettlement.ToUser;

        await _settlementRepository.UpdateSettlement(currentSettlement);

        return Result<Settlement>.Success(currentSettlement, "Settlement updated successfully.");
    }

    public async Task DeleteSettlement(Guid settlementId)
    {
        await _settlementRepository.DeleteSettlement(settlementId);
    }

    #region Private Methods

    private static Result<bool> ValidateSettlementFields(Settlement settlement)
    {
        if (settlement.Amount <= 0)
            return Result<bool>.Fail("Settlement amount must be greater than zero.", 400);

        if (settlement.Date == DateTime.MinValue)
            return Result<bool>.Fail("Settlement date is required.", 400);

        return Result<bool>.Success(true);
    }

    #endregion Private Methods
}
