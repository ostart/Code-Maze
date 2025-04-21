using CompanyEmployees.Core.Domain.Entities;
using CompanyEmployees.Core.Domain.Responses;
using OneOf;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Core.Domain.OneOfTypes;

[GenerateOneOf]
public partial class EmployeePatchTupleOrCompanyNotFoundOrEmployeeNotFound : 
    OneOfBase<(EmployeeForUpdateDto employeeToPatch, Employee? employeeEntity),
        CompanyNotFoundResponse, EmployeeNotFoundResponse>
{
}