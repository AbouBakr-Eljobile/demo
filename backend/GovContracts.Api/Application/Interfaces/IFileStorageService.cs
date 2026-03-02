using Microsoft.AspNetCore.Http;

namespace GovContracts.Api.Application.Interfaces;

public interface IFileStorageService
{
    Task<(string FileName, string RelativePath)> SaveContractAttachmentAsync(IFormFile file, CancellationToken cancellationToken = default);
}
