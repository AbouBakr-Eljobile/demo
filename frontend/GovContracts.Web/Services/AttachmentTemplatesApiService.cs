using System.Net.Http.Json;
using GovContracts.Web.Models;

namespace GovContracts.Web.Services;

public class AttachmentTemplatesApiService
{
    private readonly HttpClient _httpClient;
    private readonly AuthSessionService _authSessionService;

    public AttachmentTemplatesApiService(HttpClient httpClient, AuthSessionService authSessionService)
    {
        _httpClient = httpClient;
        _authSessionService = authSessionService;
    }

    public async Task<IReadOnlyCollection<AttachmentTemplateModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await AttachBearerAsync();
        var data = await _httpClient.GetFromJsonAsync<List<AttachmentTemplateModel>>("api/attachment-templates", cancellationToken);
        return data ?? [];
    }

    public async Task<AttachmentTemplateModel?> CreateAsync(string name, bool isRequired, CancellationToken cancellationToken = default)
    {
        await AttachBearerAsync();
        var response = await _httpClient.PostAsJsonAsync("api/attachment-templates", new { name, isRequired }, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<AttachmentTemplateModel>(cancellationToken: cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await AttachBearerAsync();
        using var response = await _httpClient.DeleteAsync($"api/attachment-templates/{id}", cancellationToken);
        return response.IsSuccessStatusCode;
    }

    private async Task AttachBearerAsync()
    {
        var token = await _authSessionService.GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            return;
        }

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }
}
