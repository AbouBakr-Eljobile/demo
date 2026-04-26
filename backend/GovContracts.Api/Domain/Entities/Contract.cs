namespace GovContracts.Api.Domain.Entities;

public class Contract
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ContractName { get; set; } = string.Empty;
    public string ContractDescription { get; set; } = string.Empty;
    public DateOnly ContractDate { get; set; }
    public decimal ContractValue { get; set; }
    public List<ContractAttachment> Attachments { get; set; } = [];
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
