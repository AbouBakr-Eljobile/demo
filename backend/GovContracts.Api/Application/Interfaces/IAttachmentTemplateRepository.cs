using GovContracts.Api.Domain.Entities;

namespace GovContracts.Api.Application.Interfaces;

public interface IAttachmentTemplateRepository
{
    Task<IReadOnlyCollection<AttachmentTemplate>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<AttachmentTemplate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<AttachmentTemplate> CreateAsync(AttachmentTemplate template, CancellationToken cancellationToken = default);
    Task<AttachmentTemplate?> UpdateAsync(AttachmentTemplate template, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
