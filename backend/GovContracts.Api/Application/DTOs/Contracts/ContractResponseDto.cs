namespace GovContracts.Api.Application.DTOs.Contracts;

public class ContractResponseDto
{
    public Guid Id { get; set; }
    public string ContractName { get; set; } = string.Empty;
    public string ContractDescription { get; set; } = string.Empty;
    public DateOnly ContractDate { get; set; }
    public decimal ContractValue { get; set; }
    public List<ContractAttachmentResponseDto> Attachments { get; set; } = [];
    public DateTime CreatedAtUtc { get; set; }
}
