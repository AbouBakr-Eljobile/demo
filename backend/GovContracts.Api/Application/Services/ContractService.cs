using GovContracts.Api.Application.DTOs.Contracts;
using GovContracts.Api.Application.Interfaces;
using GovContracts.Api.Domain.Entities;

namespace GovContracts.Api.Application.Services;

public class ContractService : IContractService
{
    private readonly IContractRepository _contractRepository;
    private readonly IFileStorageService _fileStorageService;

    public ContractService(IContractRepository contractRepository, IFileStorageService fileStorageService)
    {
        _contractRepository = contractRepository;
        _fileStorageService = fileStorageService;
    }

    public async Task<IReadOnlyCollection<ContractResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var contracts = await _contractRepository.GetAllAsync(cancellationToken);
        return contracts.Select(MapToResponse).ToList();
    }

    public async Task<ContractResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contract = await _contractRepository.GetByIdAsync(id, cancellationToken);
        return contract is null ? null : MapToResponse(contract);
    }

    public async Task<ContractResponseDto> CreateAsync(ContractCreateDto dto, string baseUrl, CancellationToken cancellationToken = default)
    {
        var contract = new Contract
        {
            ContractName = dto.ContractName,
            ContractDescription = dto.ContractDescription,
            ContractDate = dto.ContractDate,
            ContractValue = dto.ContractValue,
            Attachments = await MapAttachmentsAsync(dto.Attachments, baseUrl, cancellationToken)
        };

        var created = await _contractRepository.CreateAsync(contract, cancellationToken);
        return MapToResponse(created);
    }

    public async Task<ContractResponseDto?> UpdateAsync(Guid id, ContractUpdateDto dto, string baseUrl, CancellationToken cancellationToken = default)
    {
        var current = await _contractRepository.GetByIdAsync(id, cancellationToken);
        if (current is null)
        {
            return null;
        }

        current.ContractName = dto.ContractName;
        current.ContractDescription = dto.ContractDescription;
        current.ContractDate = dto.ContractDate;
        current.ContractValue = dto.ContractValue;
        current.Attachments = await MapAttachmentsAsync(dto.Attachments, baseUrl, cancellationToken);

        var updated = await _contractRepository.UpdateAsync(current, cancellationToken);
        return updated is null ? null : MapToResponse(updated);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _contractRepository.DeleteAsync(id, cancellationToken);
    }

    private static ContractResponseDto MapToResponse(Contract contract) => new()
    {
        Id = contract.Id,
        ContractName = contract.ContractName,
        ContractDescription = contract.ContractDescription,
        ContractDate = contract.ContractDate,
        ContractValue = contract.ContractValue,
        Attachments = contract.Attachments
            .Select(attachment => new ContractAttachmentResponseDto
            {
                TemplateId = attachment.TemplateId,
                AttachmentName = attachment.AttachmentName,
                IsRequired = attachment.IsRequired,
                StoredFileName = attachment.StoredFileName,
                AttachmentUrl = attachment.AttachmentUrl
            })
            .ToList(),
        CreatedAtUtc = contract.CreatedAtUtc
    };

    private async Task<List<ContractAttachment>> MapAttachmentsAsync(
        List<ContractAttachmentCreateDto> attachments,
        string baseUrl,
        CancellationToken cancellationToken)
    {
        var results = new List<ContractAttachment>();
        foreach (var attachment in attachments)
        {
            var result = new ContractAttachment
            {
                TemplateId = attachment.TemplateId,
                AttachmentName = attachment.AttachmentName,
                IsRequired = attachment.IsRequired
            };

            if (attachment.AttachmentFile is not null)
            {
                var fileResult = await _fileStorageService.SaveContractAttachmentAsync(attachment.AttachmentFile, cancellationToken);
                result.StoredFileName = fileResult.FileName;
                result.AttachmentUrl = BuildAttachmentUrl(baseUrl, fileResult.RelativePath);
            }

            results.Add(result);
        }

        return results;
    }

    private static string BuildAttachmentUrl(string baseUrl, string relativePath)
    {
        var normalizedBase = baseUrl.TrimEnd('/');
        var normalizedPath = relativePath.Replace("\\", "/").TrimStart('/');
        return $"{normalizedBase}/{normalizedPath}";
    }
}
