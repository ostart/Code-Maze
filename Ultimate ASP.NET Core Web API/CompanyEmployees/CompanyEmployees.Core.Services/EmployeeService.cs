using AutoMapper;
using CompanyEmployees.Core.Domain.Entities;
using CompanyEmployees.Core.Domain.Exceptions;
using CompanyEmployees.Core.Domain.LinkModels;
using CompanyEmployees.Core.Domain.Repositories;
using CompanyEmployees.Core.Services.Abstractions;
using LoggingService;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace CompanyEmployees.Core.Services;

internal sealed class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IEmployeeLinks _employeeLinks;
    
    public EmployeeService(IRepositoryManager repository, ILoggerManager logger, 
        IMapper mapper, IEmployeeLinks employeeLinks)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _employeeLinks = employeeLinks;
    }
    
    public async Task<(LinkResponse linkResponse, MetaData metaData)> GetEmployeesAsync
    (Guid companyId, LinkParameters linkParameters, bool trackChanges,
        CancellationToken ct = default)
    {
        if (!linkParameters.EmployeeParameters.ValidAgeRange)
            throw new MaxAgeRangeBadRequestException();

        await CheckIfCompanyExists(companyId, trackChanges, ct);

        var employeesWithMetaData = await _repository.Employee
            .GetEmployeesAsync(companyId, linkParameters.EmployeeParameters,
                trackChanges, ct);

        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);
            
        var links = _employeeLinks.TryGenerateLinks(employeesDto, 
            linkParameters.EmployeeParameters.Fields!, companyId, linkParameters.Context);

        return (linkResponse: links, metaData: employeesWithMetaData.MetaData);
    }

    public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges, 
        CancellationToken ct)
    {
        await CheckIfCompanyExists(companyId, trackChanges, ct);

        var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges, ct);

        var employee = _mapper.Map<EmployeeDto>(employeeDb);
        return employee;
    }

    public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, 
        EmployeeForCreationDto employeeForCreation, bool trackChanges, CancellationToken ct)
    {
        await CheckIfCompanyExists(companyId, trackChanges, ct);

        var employeeEntity = _mapper.Map<Employee>(employeeForCreation);

        _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
        await _repository.SaveAsync(ct);

        var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

        return employeeToReturn;
    }

    public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges, 
        CancellationToken ct)
    {
        var company = await CheckIfCompanyExists(companyId, trackChanges, ct);

        var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id,
            trackChanges, ct);

        await _repository.Employee.DeleteEmployeeAsync(company, employeeDb, ct);

        await _repository.SaveAsync(ct);
    }

    public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, 
        EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, 
        bool empTrackChanges, CancellationToken ct)
    {
        await CheckIfCompanyExists(companyId, compTrackChanges, ct);

        var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id,
            empTrackChanges, ct);

        _mapper.Map(employeeForUpdate, employeeDb);

        await _repository.SaveAsync(ct);
    }

    public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync
        (Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges, CancellationToken ct)
    {
        await CheckIfCompanyExists(companyId, compTrackChanges, ct);

        var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id,
            empTrackChanges, ct);

        var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeDb);

        return (employeeToPatch, employeeDb);
    }

    public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity, 
        CancellationToken ct = default)
    {
        _mapper.Map(employeeToPatch, employeeEntity);

        await _repository.SaveAsync(ct);
    }
    
    private async Task<Company> CheckIfCompanyExists(Guid companyId, bool trackChanges, CancellationToken ct)
    {
        var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges, ct);
        if (company is null)
            throw new CompanyNotFoundException(companyId);
        return company;
    }

    private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExists(Guid companyId, 
        Guid id, bool trackChanges, CancellationToken ct)
    {
        var employeeDb = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges, ct);
        if (employeeDb is null)
            throw new EmployeeNotFoundException(id);
        return employeeDb;
    }
}