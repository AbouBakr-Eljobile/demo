using Microsoft.EntityFrameworkCore;
using GovContracts.Api.Application.Interfaces;
using GovContracts.Api.Domain.Entities;
using GovContracts.Api.Infrastructure.Persistence;

namespace GovContracts.Api.Infrastructure.Repositories;

public class SqlAttachmentTemplateRepository : IAttachmentTemplateRepository
{
    private readonly ApplicationDbContext _context;

    public SqlAttachmentTemplateRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<AttachmentTemplate>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.AttachmentTemplates.ToListAsync(cancellationToken);
    }

    public async Task<AttachmentTemplate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.AttachmentTemplates.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<AttachmentTemplate> CreateAsync(AttachmentTemplate template, CancellationToken cancellationToken = default)
    {
        await _context.AttachmentTemplates.AddAsync(template, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return template;
    }

    public async Task<AttachmentTemplate?> UpdateAsync(AttachmentTemplate template, CancellationToken cancellationToken = default)
    {
        _context.AttachmentTemplates.Update(template);
        await _context.SaveChangesAsync(cancellationToken);
        return template;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var template = await _context.AttachmentTemplates.FindAsync(new object[] { id }, cancellationToken);
        if (template != null)
        {
            _context.AttachmentTemplates.Remove(template);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        return false;
    }
}