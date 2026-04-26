using GovContracts.Api.Application.DTOs.Attachments;
using GovContracts.Api.Application.Interfaces;
using GovContracts.Api.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GovContracts.Api.Controllers;

[ApiController]
[Route("api/attachment-templates")]
public class AttachmentTemplatesController : ControllerBase
{
    private readonly IAttachmentTemplateRepository _repository;

    public AttachmentTemplatesController(IAttachmentTemplateRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<AttachmentTemplateResponseDto>>> GetAll(CancellationToken cancellationToken)
    {
        var items = await _repository.GetAllAsync(cancellationToken);
        return Ok(items.Select(MapToResponse).ToList());
    }

    [HttpPost]
    public async Task<ActionResult<AttachmentTemplateResponseDto>> Create(AttachmentTemplateCreateDto request, CancellationToken cancellationToken)
    {
        var template = new AttachmentTemplate
        {
            Name = request.Name,
            IsRequired = request.IsRequired
        };

        var created = await _repository.CreateAsync(template, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, MapToResponse(created));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AttachmentTemplateResponseDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var template = await _repository.GetByIdAsync(id, cancellationToken);
        if (template is null)
        {
            return NotFound();
        }

        return Ok(MapToResponse(template));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<AttachmentTemplateResponseDto>> Update(Guid id, AttachmentTemplateUpdateDto request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            return NotFound();
        }

        existing.Name = request.Name;
        existing.IsRequired = request.IsRequired;

        var updated = await _repository.UpdateAsync(existing, cancellationToken);
        if (updated is null)
        {
            return NotFound();
        }

        return Ok(MapToResponse(updated));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    private static AttachmentTemplateResponseDto MapToResponse(AttachmentTemplate template) => new()
    {
        Id = template.Id,
        Name = template.Name,
        IsRequired = template.IsRequired,
        CreatedAtUtc = template.CreatedAtUtc
    };
}
