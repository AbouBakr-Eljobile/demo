namespace GovContracts.Web.Models;

public class ContractModel
{
    public Guid Id { get; set; }
    public string ContractName { get; set; } = string.Empty;
    public string ContractDescription { get; set; } = string.Empty;
    public DateTime ContractDate { get; set; }
    public decimal ContractValue { get; set; }
    public List<ContractAttachmentModel> Attachments { get; set; } = [];
    public DateTime CreatedAtUtc { get; set; }
}
