using FairShare.Domain.Models;

namespace FairShare.APIClient.Contracts;

public interface ISettlementsApiService
{
    Task<Settlement> GetSettlementById(Guid settlementId);
    Task<IEnumerable<Settlement>> GetSettlementsByGroupId(Guid groupId);
    Task<IEnumerable<Settlement>> GetSettlementsFromUserId(Guid userId);
    Task<IEnumerable<Settlement>> GetSettlementsToUserId(Guid userId);
    Task<Settlement> CreateSettlement(Settlement settlement);
    Task UpdateSettlement(Settlement settlement);
    Task DeleteSettlement(Guid settlementId);
}
