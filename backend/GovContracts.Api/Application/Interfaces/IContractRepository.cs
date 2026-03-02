using GovContracts.Api.Domain.Entities;

namespace GovContracts.Api.Application.Interfaces;

public interface IContractRepository
{
    Task<IReadOnlyCollection<Contract>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Contract?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Contract> CreateAsync(Contract contract, CancellationToken cancellationToken = default);
    Task<Contract?> UpdateAsync(Contract contract, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
