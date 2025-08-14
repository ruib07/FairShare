using FairShare.APIClient.Contracts;
using FairShare.Domain.Models;
using System.Net.Http.Json;

namespace FairShare.APIClient;

public class ExpensesApiService : IExpensesApiService
{
    private readonly HttpClient _httpClient;
    private const string _baseUrl = "expenses";

    public ExpensesApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("FairShareApi");
    }

    public async Task<Expense> GetExpenseById(Guid expenseId)
    {
        return await _httpClient.GetFromJsonAsync<Expense>($"{_baseUrl}/{expenseId}");
    }

    public async Task<IEnumerable<Expense>> GetExpensesByGroupId(Guid groupId)
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<Expense>>($"{_baseUrl}/by-group/{groupId}");
        return result ?? Enumerable.Empty<Expense>();
    }

    public async Task<IEnumerable<Expense>> GetPaidExpensesByUserId(Guid userId)
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<Expense>>($"{_baseUrl}/paid/{userId}");
        return result ?? Enumerable.Empty<Expense>();
    }

    public async Task<Expense> CreateExpense(Expense expense)
    {
        var response = await _httpClient.PostAsJsonAsync(_baseUrl, expense);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Expense>();
    }

    public async Task UpdateExpense(Expense expense)
    {
        var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{expense.Id}", expense);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteExpense(Guid expenseId)
    {
        var response = await _httpClient.DeleteAsync($"{_baseUrl}/{expenseId}");
        response.EnsureSuccessStatusCode();
    }
}
}
