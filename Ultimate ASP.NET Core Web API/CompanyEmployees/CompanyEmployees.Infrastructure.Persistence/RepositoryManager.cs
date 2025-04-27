using CompanyEmployees.Core.Domain.Repositories;
using CompanyEmployees.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace CompanyEmployees.Infrastructure.Persistence;

public sealed class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _repositoryContext;
    private readonly ExternalClientContext _externalClientContext;

    private readonly Lazy<ICompanyRepository> _companyRepository;
    private readonly Lazy<IEmployeeRepository> _employeeRepository;
    private readonly Lazy<IClientRepository> _clientRepository;

    public RepositoryManager(RepositoryContext repositoryContext, 
        ExternalClientContext externalClientContext)
    {
        _repositoryContext = repositoryContext;
        _externalClientContext = externalClientContext;

        _companyRepository = new Lazy<ICompanyRepository>(() => 
            new CompanyRepository(repositoryContext));
        _employeeRepository = new Lazy<IEmployeeRepository>(() => 
            new EmployeeRepository(repositoryContext));
        _clientRepository = new Lazy<IClientRepository>(() => 
            new ClientRepository(externalClientContext));
    }

    public ICompanyRepository Company => _companyRepository.Value;
    public IEmployeeRepository Employee => _employeeRepository.Value;
    public IClientRepository Client => _clientRepository.Value;

    public async Task SaveAsync(CancellationToken ct = default)
    {
        if (_repositoryContext.ChangeTracker.HasChanges())
            await _repositoryContext.SaveChangesAsync(ct);
        else
            await _externalClientContext.SaveChangesAsync(ct);
    }
}