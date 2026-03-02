namespace GovContracts.Web.Models;

public class ContractModel
{
    public Guid Id { get; set; }
    public string ContractName { get; set; } = string.Empty;
    public DateTime ContractDate { get; set; }
    public string? AttachmentFileName { get; set; }
    public string? AttachmentUrl { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
