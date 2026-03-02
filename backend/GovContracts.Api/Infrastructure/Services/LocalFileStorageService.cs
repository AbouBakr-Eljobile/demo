using GovContracts.Api.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GovContracts.Api.Infrastructure.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _environment;

    public LocalFileStorageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<(string FileName, string RelativePath)> SaveContractAttachmentAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        var uploadsDirectory = Path.Combine(_environment.ContentRootPath, "uploads", "contracts");
        Directory.CreateDirectory(uploadsDirectory);

        var extension = Path.GetExtension(file.FileName);
        var uniqueName = $"{Guid.NewGuid():N}{extension}";
        var absolutePath = Path.Combine(uploadsDirectory, uniqueName);

        await using var stream = File.Create(absolutePath);
        await file.CopyToAsync(stream, cancellationToken);

        return (file.FileName, $"uploads/contracts/{uniqueName}");
    }
}
