using GovContracts.Web.Models;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace GovContracts.Web.Services;

public class ContractsApiService
{
    private readonly HttpClient _httpClient;
    private readonly AuthSessionService _authSessionService;

    public ContractsApiService(HttpClient httpClient, AuthSessionService authSessionService)
    {
        _httpClient = httpClient;
        _authSessionService = authSessionService;
    }

    public async Task<IReadOnlyCollection<ContractModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await AttachBearerAsync();
        var data = await _httpClient.GetFromJsonAsync<List<ContractModel>>("api/contracts", cancellationToken);
        return data ?? [];
    }

    public async Task<bool> CreateAsync(string contractName, DateTime contractDate, IBrowserFile? contractAttachment, CancellationToken cancellationToken = default)
    {
        await AttachBearerAsync();

        using var formData = new MultipartFormDataContent();
        formData.Add(new StringContent(contractName), "contractName");
        formData.Add(new StringContent(contractDate.ToString("yyyy-MM-dd")), "contractDate");

        if (contractAttachment is not null)
        {
            await using var stream = contractAttachment.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024, cancellationToken);
            using var memory = new MemoryStream();
            await stream.CopyToAsync(memory, cancellationToken);
            memory.Position = 0;

            var fileContent = new ByteArrayContent(memory.ToArray());
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(contractAttachment.ContentType);
            formData.Add(fileContent, "contractAttachment", contractAttachment.Name);
        }

        using var response = await _httpClient.PostAsync("api/contracts", formData, cancellationToken);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await AttachBearerAsync();
        using var response = await _httpClient.DeleteAsync($"api/contracts/{id}", cancellationToken);
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

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
