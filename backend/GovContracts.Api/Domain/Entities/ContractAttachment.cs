namespace GovContracts.Api.Domain.Entities;

public class ContractAttachment
{
    public Guid? TemplateId { get; set; }
    public string AttachmentName { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public string? StoredFileName { get; set; }
    public string? AttachmentUrl { get; set; }
}
