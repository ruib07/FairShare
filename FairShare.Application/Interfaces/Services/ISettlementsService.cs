using FairShare.Application.Shared.Common;
using FairShare.Domain.Models;

namespace FairShare.Application.Interfaces.Services;

public interface ISettlementsService
{
    Task<Result<Settlement>> GetSettlementById(Guid settlementId);
    Task<IEnumerable<Settlement>> GetSettlementsByGroupId(Guid groupId);
    Task<IEnumerable<Settlement>> GetSettlementsFromUserId(Guid userId);
    Task<IEnumerable<Settlement>> GetSettlementsToUserId(Guid userId);
    Task<Result<Settlement>> CreateSettlement(Settlement settlement);
    Task<Result<Settlement>> UpdateSettlement(Guid settlementId, Settlement updateSettlement);
    Task DeleteSettlement(Guid settlementId);
}
