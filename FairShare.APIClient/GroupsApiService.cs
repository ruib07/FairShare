using FairShare.APIClient.Contracts;
using FairShare.Domain.Models;
using System.Net.Http.Json;

namespace FairShare.APIClient;

public class GroupsApiService : IGroupsApiService
{
    private readonly HttpClient _httpClient;
    private const string _baseUrl = "groups";

    public GroupsApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("FairShareApi");
    }

    public async Task<Group> GetGroupById(Guid groupId)
    {
        return await _httpClient.GetFromJsonAsync<Group>($"{_baseUrl}/{groupId}");
    }

    public async Task<IEnumerable<Group>> GetGroupsCreatedByUserId(Guid userId)
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<Group>>($"{_baseUrl}/created-by/{userId}");
        return result ?? [];
    }

    public async Task<Group> CreateGroup(Group group)
    {
        var response = await _httpClient.PostAsJsonAsync(_baseUrl, group);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Group>();
    }

    public async Task UpdateGroup(Group group)
    {
        var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{group.Id}", group);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteGroup(Guid groupId)
    {
        var response = await _httpClient.DeleteAsync($"{_baseUrl}/{groupId}");
        response.EnsureSuccessStatusCode();
    }
}
}
