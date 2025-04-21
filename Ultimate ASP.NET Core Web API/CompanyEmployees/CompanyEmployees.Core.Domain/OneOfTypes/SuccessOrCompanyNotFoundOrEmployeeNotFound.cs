using CompanyEmployees.Core.Domain.Responses;
using OneOf;
using OneOf.Types;

namespace CompanyEmployees.Core.Domain.OneOfTypes;

[GenerateOneOf]
public partial class SuccessOrCompanyNotFoundOrEmployeeNotFound : 
    OneOfBase<Success, CompanyNotFoundResponse, EmployeeNotFoundResponse>
{
    
}