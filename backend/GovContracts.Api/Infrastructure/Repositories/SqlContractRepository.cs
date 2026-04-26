using Microsoft.EntityFrameworkCore;
using GovContracts.Api.Application.Interfaces;
using GovContracts.Api.Domain.Entities;
using GovContracts.Api.Infrastructure.Persistence;

namespace GovContracts.Api.Infrastructure.Repositories;

public class SqlContractRepository : IContractRepository
{
    private readonly ApplicationDbContext _context;

    public SqlContractRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<Contract>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Contracts.OrderByDescending(c => c.CreatedAtUtc).ToListAsync(cancellationToken);
    }

    public async Task<Contract?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Contracts.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<Contract> CreateAsync(Contract contract, CancellationToken cancellationToken = default)
    {
        await _context.Contracts.AddAsync(contract, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return contract;
    }

    public async Task<Contract?> UpdateAsync(Contract contract, CancellationToken cancellationToken = default)
    {
        _context.Contracts.Update(contract);
        await _context.SaveChangesAsync(cancellationToken);
        return contract;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contract = await _context.Contracts.FindAsync(new object[] { id }, cancellationToken);
        if (contract != null)
        {
            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        return false;
    }
}