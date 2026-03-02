using GovContracts.Api.Application.DTOs.Contracts;

namespace GovContracts.Api.Application.Interfaces;

public interface IContractService
{
    Task<IReadOnlyCollection<ContractResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ContractResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ContractResponseDto> CreateAsync(ContractCreateDto dto, string baseUrl, CancellationToken cancellationToken = default);
    Task<ContractResponseDto?> UpdateAsync(Guid id, ContractUpdateDto dto, string baseUrl, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
