namespace GovContracts.Api.Application.DTOs.Attachments;

public class AttachmentTemplateUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
}
