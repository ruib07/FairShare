using FairShare.APIClient.Contracts;
using System.Net.Http.Json;

namespace FairShare.APIClient;

public class GroupMembersApiService : IGroupMembersApiService
{
    private readonly HttpClient _httpClient;
    private const string _baseUrl = "group-members";

    public GroupMembersApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("FairShareApi");
    }

    public async Task<IEnumerable<Guid>> GetGroupsForUser(Guid userId)
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<Guid>>($"{_baseUrl}/for-user/{userId}");
        return result ?? [];
    }

    public async Task<IEnumerable<Guid>> GetUsersInGroup(Guid groupId)
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<Guid>>($"{_baseUrl}/in-group/{groupId}");
        return result ?? [];
    }

    public async Task<bool> IsUserInGroup(Guid groupId, Guid userId)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/{groupId}/is-in-group/{userId}");

        if (!response.IsSuccessStatusCode)
            return false;

        return await response.Content.ReadFromJsonAsync<bool>();
    }

    public async Task AddUserToGroup(Guid groupId, Guid userId)
    {
        var response = await _httpClient.PostAsync($"{_baseUrl}/{groupId}/users/{userId}", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveUserFromGroup(Guid groupId, Guid userId)
    {
        var response = await _httpClient.DeleteAsync($"{_baseUrl}/{groupId}/users/{userId}");
        response.EnsureSuccessStatusCode();
    }
}
}
