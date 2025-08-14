using FairShare.APIClient.Contracts;
using FairShare.Domain.Models;
using System.Net.Http.Json;

namespace FairShare.APIClient;

public class ExpenseParticipantsApiService : IExpenseParticipantsApiService
{
    private readonly HttpClient _httpClient;
    private const string _baseUrl = "expense-participants";

    public ExpenseParticipantsApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("FairShareApi");
    }

    public async Task<IEnumerable<Guid>> GetParticipantsByExpenseId(Guid expenseId)
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<Guid>>(
            $"/{_baseUrl}/{expenseId}/participants");

        return result ?? [];
    }

    public async Task<bool> IsUserParticipating(Guid expenseId, Guid userId)
    {
        var response = await _httpClient.GetAsync(
            $"/{_baseUrl}/{expenseId}/is-participating/{userId}");

        if (!response.IsSuccessStatusCode) return false;

        return await response.Content.ReadFromJsonAsync<bool>();
    }

    public async Task AddParticipant(ExpenseParticipant expenseParticipant)
    {
        var response = await _httpClient.PostAsJsonAsync(_baseUrl, expenseParticipant);
        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveParticipant(Guid expenseId, Guid userId)
    {
        var response = await _httpClient.DeleteAsync($"/{_baseUrl}/{expenseId}/{userId}");
        response.EnsureSuccessStatusCode();
    }
}
