using CompanyEmployees.Core.Domain.Entities;

namespace CompanyEmployees.Core.Domain.Repositories;

public interface IClientRepository
{
    Task<IEnumerable<Client>> GetAllClientsAsync(bool trackChanges, CancellationToken ct = default);
    void CreateClient(Client client);
}