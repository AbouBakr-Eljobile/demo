using Microsoft.JSInterop;

namespace GovContracts.Web.Services;

public class AuthSessionService
{
    private const string TokenKey = "govcontracts.token";
    private const string DisplayNameKey = "govcontracts.displayName";

    private readonly IJSRuntime _jsRuntime;

    public AuthSessionService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SetSessionAsync(string token, string displayName)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", DisplayNameKey, displayName);
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", TokenKey);
    }

    public async Task<string?> GetDisplayNameAsync()
    {
        return await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", DisplayNameKey);
    }

    public async Task<bool> IsLoggedInAsync()
    {
        var token = await GetTokenAsync();
        return !string.IsNullOrWhiteSpace(token);
    }

    public async Task ClearAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", DisplayNameKey);
    }
}
