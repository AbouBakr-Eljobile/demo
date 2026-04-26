using GovContracts.Api.Application.DTOs.Contracts;
using GovContracts.Api.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GovContracts.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContractsController : ControllerBase
{
    private readonly IContractService _contractService;
    private readonly IAttachmentTemplateRepository _attachmentTemplateRepository;

    public ContractsController(IContractService contractService, IAttachmentTemplateRepository attachmentTemplateRepository)
    {
        _contractService = contractService;
        _attachmentTemplateRepository = attachmentTemplateRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<ContractResponseDto>>> GetAll(CancellationToken cancellationToken)
    {
        var data = await _contractService.GetAllAsync(cancellationToken);
        return Ok(data);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ContractResponseDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var contract = await _contractService.GetByIdAsync(id, cancellationToken);
        if (contract is null)
        {
            return NotFound();
        }

        return Ok(contract);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ContractResponseDto>> Create([FromForm] ContractCreateDto request, CancellationToken cancellationToken)
    {
        if (HasMissingRequiredAttachments(request.Attachments))
        {
            return BadRequest("Missing required attachments.");
        }

        if (await HasMissingRequiredTemplatesAsync(request.Attachments, cancellationToken))
        {
            return BadRequest("Missing required attachment templates.");
        }

        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var created = await _contractService.CreateAsync(request, baseUrl, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ContractResponseDto>> Update(Guid id, [FromForm] ContractUpdateDto request, CancellationToken cancellationToken)
    {
        if (HasMissingRequiredAttachments(request.Attachments))
        {
            return BadRequest("Missing required attachments.");
        }

        if (await HasMissingRequiredTemplatesAsync(request.Attachments, cancellationToken))
        {
            return BadRequest("Missing required attachment templates.");
        }

        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var updated = await _contractService.UpdateAsync(id, request, baseUrl, cancellationToken);
        if (updated is null)
        {
            return NotFound();
        }

        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var isDeleted = await _contractService.DeleteAsync(id, cancellationToken);
        if (!isDeleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    private static bool HasMissingRequiredAttachments(IEnumerable<ContractAttachmentCreateDto> attachments)
    {
        return attachments.Any(attachment => attachment.IsRequired && attachment.AttachmentFile is null);
    }

    private async Task<bool> HasMissingRequiredTemplatesAsync(
        IEnumerable<ContractAttachmentCreateDto> attachments,
        CancellationToken cancellationToken)
    {
        var templates = await _attachmentTemplateRepository.GetAllAsync(cancellationToken);
        var requiredTemplates = templates.Where(template => template.IsRequired).ToList();
        if (requiredTemplates.Count == 0)
        {
            return false;
        }

        var attachmentsByTemplateId = attachments
            .Where(attachment => attachment.TemplateId.HasValue)
            .ToDictionary(attachment => attachment.TemplateId!.Value, attachment => attachment);

        foreach (var template in requiredTemplates)
        {
            if (!attachmentsByTemplateId.TryGetValue(template.Id, out var attachment))
            {
                return true;
            }

            if (attachment.AttachmentFile is null)
            {
                return true;
            }
        }

        return false;
    }
}
