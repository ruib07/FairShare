using FairShare.APIClient.Contracts;
using FairShare.Application.Shared.DTOs;
using System.Net.Http.Json;

namespace FairShare.APIClient;

public class AuthenticationsApiService : IAuthenticationsApiService
{
    private readonly HttpClient _httpClient;
    private const string _baseUrl = "authentications";

    public AuthenticationsApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("FairShareApi");
    }

    public async Task SignUp(AuthenticationsDTO.SignUp.Request signup)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/signup", signup);
        response.EnsureSuccessStatusCode();
    }

    public async Task<AuthenticationsDTO.SignIn.Response> SignIn(AuthenticationsDTO.SignIn.Request signin)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/signin", signin);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AuthenticationsDTO.SignIn.Response>();

        if (result == null)
            throw new InvalidOperationException("Failed to parse sign-in response.");

        return result;
    }

    public async Task<AuthenticationsDTO.SignIn.Response> RefreshToken(AuthenticationsDTO.SignIn.RefreshTokenRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.RefreshToken))
            throw new ArgumentException("Refresh token is required.", nameof(request));

        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/refresh", request);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AuthenticationsDTO.SignIn.Response>();

        return result ?? throw new InvalidOperationException("Failed to parse refresh token response.");
    }

    public async Task LogOut(AuthenticationsDTO.SignIn.RefreshTokenRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/logout", request);
        response.EnsureSuccessStatusCode();
    }
}
