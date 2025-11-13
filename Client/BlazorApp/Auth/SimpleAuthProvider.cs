using System;
using System.Security.Claims;
using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using ApiContracts;

namespace BlazorApp.Auth;
public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient httpClient;
    private readonly IJSRuntime jsRuntime;

    // 🔹 Primary cache — the in-memory copy of the current user
    private string? _cachedUserJson;

    public SimpleAuthProvider(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        this.httpClient = httpClient;
        this.jsRuntime = jsRuntime;
    }

    public async Task LoginASync(string userName, string password)
    {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(
            "auth/login",
            new LoginRequest(userName, password));

        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        UserDto userDto = JsonSerializer.Deserialize<UserDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;

        // 🔹 Serialize and store user both in cache and sessionStorage
        _cachedUserJson = JsonSerializer.Serialize(userDto);
        await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", _cachedUserJson);

        // 🔹 Build claims
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, userDto.UserName),
            new(ClaimTypes.NameIdentifier, userDto.Id.ToString())
        };

        var identity = new ClaimsIdentity(claims, "apiauth");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        // 🔹 Notify UI
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string? userAsJson = _cachedUserJson;

        // 🔹 If memory cache is empty, look in sessionStorage
        if (string.IsNullOrEmpty(userAsJson))
        {
            try
            {
                userAsJson = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
                _cachedUserJson = userAsJson; // Cache for next time
            }
            catch (InvalidOperationException)
            {
                // Happens during prerendering on Blazor Server or restricted JS runtime contexts
                return new AuthenticationState(new());
            }
        }

        if (string.IsNullOrEmpty(userAsJson))
        {
            // 🔹 No user logged in
            return new AuthenticationState(new());
        }

        // 🔹 Deserialize and create ClaimsPrincipal
        UserDto? userDto = JsonSerializer.Deserialize<UserDto>(userAsJson);
        if (userDto is null)
            return new AuthenticationState(new());

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, userDto.UserName),
            new(ClaimTypes.NameIdentifier, userDto.Id.ToString())
        };

        var identity = new ClaimsIdentity(claims, "apiauth");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        return new AuthenticationState(claimsPrincipal);
    }

    public async Task Logout()
    {
        _cachedUserJson = null;
        await jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", "currentUser");
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new())));
    }
}