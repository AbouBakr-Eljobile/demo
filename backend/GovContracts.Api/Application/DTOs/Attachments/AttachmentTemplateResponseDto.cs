namespace GovContracts.Api.Application.DTOs.Attachments;

public class AttachmentTemplateResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
