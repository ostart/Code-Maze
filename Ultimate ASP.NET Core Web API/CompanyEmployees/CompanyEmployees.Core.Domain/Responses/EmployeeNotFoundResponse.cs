namespace CompanyEmployees.Core.Domain.Responses;

public class EmployeeNotFoundResponse : ApiNotFoundResponse
{
    public EmployeeNotFoundResponse(Guid id) 
        : base($"Employee with id: {id} is not found in db.")
    {
    }
}