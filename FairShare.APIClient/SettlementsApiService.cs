using FairShare.APIClient.Contracts;
using FairShare.Domain.Models;
using System.Net.Http.Json;

namespace FairShare.APIClient;

public class SettlementsApiService : ISettlementsApiService
{
    private readonly HttpClient _httpClient;
    private const string _baseUrl = "settlements";

    public SettlementsApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("FairShareApi");
    }

    public async Task<Settlement> GetSettlementById(Guid settlementId)
    {
        return await _httpClient.GetFromJsonAsync<Settlement>($"{_baseUrl}/{settlementId}");
    }

    public async Task<IEnumerable<Settlement>> GetSettlementsByGroupId(Guid groupId)
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<Settlement>>($"{_baseUrl}/group/{groupId}");
        return result ?? [];
    }

    public async Task<IEnumerable<Settlement>> GetSettlementsFromUserId(Guid userId)
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<Settlement>>($"{_baseUrl}/from/{userId}");
        return result ?? [];
    }

    public async Task<IEnumerable<Settlement>> GetSettlementsToUserId(Guid userId)
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<Settlement>>($"{_baseUrl}/to/{userId}");
        return result ?? [];
    }

    public async Task<Settlement> CreateSettlement(Settlement settlement)
    {
        var response = await _httpClient.PostAsJsonAsync(_baseUrl, settlement);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Settlement>();
    }

    public async Task UpdateSettlement(Settlement settlement)
    {
        var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{settlement.Id}", settlement);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteSettlement(Guid settlementId)
    {
        var response = await _httpClient.DeleteAsync($"{_baseUrl}/{settlementId}");
        response.EnsureSuccessStatusCode();
    }
}
