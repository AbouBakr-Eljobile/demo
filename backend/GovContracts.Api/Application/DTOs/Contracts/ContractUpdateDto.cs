using Microsoft.AspNetCore.Http;

namespace GovContracts.Api.Application.DTOs.Contracts;

public class ContractUpdateDto
{
    public string ContractName { get; set; } = string.Empty;
    public string ContractDescription { get; set; } = string.Empty;
    public DateOnly ContractDate { get; set; }
    public decimal ContractValue { get; set; }
    public List<ContractAttachmentCreateDto> Attachments { get; set; } = [];
}
