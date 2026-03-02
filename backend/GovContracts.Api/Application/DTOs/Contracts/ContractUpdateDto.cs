using Microsoft.AspNetCore.Http;

namespace GovContracts.Api.Application.DTOs.Contracts;

public class ContractUpdateDto
{
    public string ContractName { get; set; } = string.Empty;
    public DateOnly ContractDate { get; set; }
    public IFormFile? ContractAttachment { get; set; }
}
