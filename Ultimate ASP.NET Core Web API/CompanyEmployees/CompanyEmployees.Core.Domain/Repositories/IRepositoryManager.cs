namespace CompanyEmployees.Core.Domain.Repositories;

public interface IRepositoryManager
{
    ICompanyRepository Company { get; }
    IEmployeeRepository Employee { get; }
    IClientRepository Client { get; }
    Task SaveAsync(CancellationToken ct = default);
}