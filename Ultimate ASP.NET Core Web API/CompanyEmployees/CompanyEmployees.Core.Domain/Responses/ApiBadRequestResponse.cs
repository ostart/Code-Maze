namespace CompanyEmployees.Core.Domain.Responses;

public abstract class ApiBadRequestResponse
{
    public string? Message { get; set; }

    protected ApiBadRequestResponse(string? message) => Message = message;
}