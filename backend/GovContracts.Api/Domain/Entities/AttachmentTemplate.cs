namespace GovContracts.Api.Domain.Entities;

public class AttachmentTemplate
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
