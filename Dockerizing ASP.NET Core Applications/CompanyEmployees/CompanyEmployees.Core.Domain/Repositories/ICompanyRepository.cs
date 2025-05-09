﻿using CompanyEmployees.Core.Domain.Entities;

namespace CompanyEmployees.Core.Domain.ContextFactory;

public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges, CancellationToken ct = default);
    Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges, CancellationToken ct = default);
    void CreateCompany(Company company);
    Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges, CancellationToken ct = default);
    void DeleteCompany(Company company);
}
