using AutoMapper;
using CompanyEmployees.Core.Domain.Entities;
using CompanyEmployees.Core.Domain.Exceptions;
using CompanyEmployees.Core.Domain.Repositories;
using CompanyEmployees.Core.Services.Abstractions;
using LoggingService;
using Shared.DataTransferObjects;
using CompanyEmployees.Core.Domain.Responses;
using OneOf;
using OneOf.Types;
using CompanyEmployees.Core.Services.Extensions;

namespace CompanyEmployees.Core.Services;

internal sealed class CompanyService : ICompanyService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges, CancellationToken ct = default)
    {
        var companies = await _repository.Company.GetAllCompaniesAsync(trackChanges, ct);

        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

        return companiesDto;
    }
    
    public async Task<OneOf<CompanyDto, CompanyNotFoundResponse>> GetCompanyAsync(Guid id, bool trackChanges, 
        CancellationToken ct = default)
    {
        var (company, error) = await GetCompanyAndCheckIfItExists(id, trackChanges, ct);
        if (error is not null) 
            return error;

        var companyDto = _mapper.Map<CompanyDto>(company);

        return companyDto;
    }
    
    public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company, CancellationToken ct = default)
    {
        var companyEntity = _mapper.Map<Company>(company);

        _repository.Company.CreateCompany(companyEntity);
        await _repository.SaveAsync(ct);

        var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);

        return companyToReturn;
    }

    public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, 
        bool trackChanges, CancellationToken ct = default)
    {
        if (ids is null)
            throw new IdParametersBadRequestException();

        var companyEntities = await _repository.Company.GetByIdsAsync(ids, trackChanges, ct);
        if (ids.Count() != companyEntities.Count())
            throw new CollectionByIdsBadRequestException();

        var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);

        return companiesToReturn;
    }

    public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync
        (IEnumerable<CompanyForCreationDto> companyCollection, CancellationToken ct = default)
    {
        if (companyCollection is null)
            throw new CompanyCollectionBadRequest();

        var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
        foreach (var company in companyEntities)
        {
            _repository.Company.CreateCompany(company);
        }

        await _repository.SaveAsync(ct);

        var companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
        var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));

        return (companies: companyCollectionToReturn, ids: ids);
    }
    
    public async Task<OneOf<Success, CompanyNotFoundResponse>> DeleteCompanyAsync(Guid companyId, bool trackChanges, CancellationToken ct = default)
    {
        var (company, error) = await GetCompanyAndCheckIfItExists(companyId, trackChanges, ct);
        if (error is not null) 
            return error;
        
        _repository.Company.DeleteCompany(company);
        
        await _repository.SaveAsync(ct);
        
        return new Success();
    }
    
    public async Task<OneOf<Success, CompanyNotFoundResponse>> UpdateCompanyAsync(Guid companyId, 
        CompanyForUpdateDto companyForUpdate, bool trackChanges, CancellationToken ct = default)
    {
        var result = await GetCompanyAndCheckIfItExists(companyId, trackChanges, ct);
        if (result.TryPickT1(out var error, out var company))
            return error;

        _mapper.Map(companyForUpdate, company);
        await _repository.SaveAsync(ct);

        return new Success();
    }
    
    private async Task<OneOf<Company, CompanyNotFoundResponse>> GetCompanyAndCheckIfItExists(Guid id, 
        bool trackChanges, CancellationToken ct)
    {
        var company = await _repository.Company.GetCompanyAsync(id, trackChanges, ct);
        if (company is null)
            return new CompanyNotFoundResponse(id);

        return company;
    }
}