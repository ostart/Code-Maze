using CompanyEmployees.Core.Domain.Entities;
using CompanyEmployees.Core.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CompanyEmployees.Infrastructure.Persistence.Repositories;

internal sealed class ClientRepository : RepositoryBase<ExternalClientContext, Client>, IClientRepository
{
    public ClientRepository(ExternalClientContext clientContext)
        : base(clientContext)
    {
    }

    public async Task<IEnumerable<Client>> GetAllClientsAsync(bool trackChanges, CancellationToken ct = default)
        => await FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToListAsync(ct);

    public void CreateClient(Client client) => Create(client);
}