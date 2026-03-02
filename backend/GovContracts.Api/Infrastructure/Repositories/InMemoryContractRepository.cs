using GovContracts.Api.Application.Interfaces;
using GovContracts.Api.Domain.Entities;

namespace GovContracts.Api.Infrastructure.Repositories;

public class InMemoryContractRepository : IContractRepository
{
    private readonly List<Contract> _contracts = [];
    private readonly object _lock = new();

    public Task<IReadOnlyCollection<Contract>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            var snapshot = _contracts
                .OrderByDescending(x => x.CreatedAtUtc)
                .Select(Clone)
                .ToList()
                .AsReadOnly();
            return Task.FromResult((IReadOnlyCollection<Contract>)snapshot);
        }
    }

    public Task<Contract?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            var contract = _contracts.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(contract is null ? null : Clone(contract));
        }
    }

    public Task<Contract> CreateAsync(Contract contract, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            var newContract = Clone(contract);
            _contracts.Add(newContract);
            return Task.FromResult(Clone(newContract));
        }
    }

    public Task<Contract?> UpdateAsync(Contract contract, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            var index = _contracts.FindIndex(x => x.Id == contract.Id);
            if (index < 0)
            {
                return Task.FromResult<Contract?>(null);
            }

            _contracts[index] = Clone(contract);
            return Task.FromResult<Contract?>(Clone(_contracts[index]));
        }
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            var removed = _contracts.RemoveAll(x => x.Id == id) > 0;
            return Task.FromResult(removed);
        }
    }

    private static Contract Clone(Contract contract) => new()
    {
        Id = contract.Id,
        ContractName = contract.ContractName,
        ContractDate = contract.ContractDate,
        AttachmentFileName = contract.AttachmentFileName,
        AttachmentUrl = contract.AttachmentUrl,
        CreatedAtUtc = contract.CreatedAtUtc
    };
}
