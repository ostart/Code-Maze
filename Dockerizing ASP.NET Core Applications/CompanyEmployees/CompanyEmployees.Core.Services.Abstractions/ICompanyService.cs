﻿using Shared.DataTransferObjects;

namespace CompanyEmployees.Core.Services.Abstractions;

public interface ICompanyService
{
    Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges, CancellationToken ct = default);
    Task<CompanyDto> GetCompanyAsync(Guid companyId, bool trackChanges, CancellationToken ct = default);
    Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company, CancellationToken ct = default);
    Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges, CancellationToken ct = default);
    Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync
        (IEnumerable<CompanyForCreationDto> companyCollection, CancellationToken ct = default);
    Task DeleteCompanyAsync(Guid companyId, bool trackChanges, CancellationToken ct = default);
    Task UpdateCompanyAsync(Guid companyid, CompanyForUpdateDto companyForUpdate, 
        bool trackChanges, CancellationToken ct = default);
}