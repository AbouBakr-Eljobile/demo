namespace GovContracts.Api.Application.DTOs.Contracts;

public class ContractResponseDto
{
    public Guid Id { get; set; }
    public string ContractName { get; set; } = string.Empty;
    public DateOnly ContractDate { get; set; }
    public string? AttachmentFileName { get; set; }
    public string? AttachmentUrl { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
