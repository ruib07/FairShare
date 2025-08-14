using FairShare.APIClient.Contracts;
using FairShare.Domain.Models;
using System.Net.Http.Json;

namespace FairShare.APIClient;

public class UsersApiService : IUsersApiService
{
    private readonly HttpClient _httpClient;
    private const string _baseUrl = "users";

    public UsersApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("FairShareApi");
    }

    public async Task<User> GetUserById(Guid userId)
    {
        return await _httpClient.GetFromJsonAsync<User>($"{_baseUrl}/{userId}");
    }

    public async Task UpdateUser(User user)
    {
        var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{user.Id}", user);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteUser(Guid userId)
    {
        var response = await _httpClient.DeleteAsync($"{_baseUrl}/{userId}");
        response.EnsureSuccessStatusCode();
    }
}