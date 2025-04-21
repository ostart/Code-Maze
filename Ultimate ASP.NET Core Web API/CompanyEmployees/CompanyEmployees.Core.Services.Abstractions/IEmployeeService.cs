using CompanyEmployees.Core.Domain.Entities;
using CompanyEmployees.Core.Domain.LinkModels;
using CompanyEmployees.Core.Domain.OneOfTypes;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace CompanyEmployees.Core.Services.Abstractions;

public interface IEmployeeService
{
    Task<(LinkResponse linkResponse, MetaData metaData)> GetEmployeesAsync(Guid companyId, 
        LinkParameters linkParameters, bool trackChanges,
        CancellationToken ct = default);
    Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges, 
        CancellationToken ct = default);
    Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreation, 
        bool trackChanges, CancellationToken ct = default);
    Task<SuccessOrCompanyNotFoundOrEmployeeNotFound> DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges, 
        CancellationToken ct = default);
    Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id,
        EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges, 
        CancellationToken ct = default);
    Task<EmployeePatchTupleOrCompanyNotFoundOrEmployeeNotFound> GetEmployeeForPatchAsync(
        Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges,
        CancellationToken ct = default);
    Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity, 
        CancellationToken ct = default);
}