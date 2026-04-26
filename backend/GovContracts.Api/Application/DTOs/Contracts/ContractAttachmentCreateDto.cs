using Microsoft.AspNetCore.Http;

namespace GovContracts.Api.Application.DTOs.Contracts;

public class ContractAttachmentCreateDto
{
    public Guid? TemplateId { get; set; }
    public string AttachmentName { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public IFormFile? AttachmentFile { get; set; }
}
