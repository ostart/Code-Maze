using Shared.DataTransferObjects;

namespace CompanyEmployees.Core.Services.Abstractions;

public interface IClientService
{
    Task<IEnumerable<ClientDto>> GetAllClientsAsync(bool trackChanges, CancellationToken ct = default);
    Task<ClientDto> CreateClientAsync(ClientForCreationDto company, CancellationToken ct = default);
}