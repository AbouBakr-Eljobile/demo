using Microsoft.AspNetCore.Components.Forms;

namespace GovContracts.Web.Models;

public class ContractAttachmentInputModel
{
    public Guid? TemplateId { get; set; }
    public string AttachmentName { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public IBrowserFile? AttachmentFile { get; set; }
}
