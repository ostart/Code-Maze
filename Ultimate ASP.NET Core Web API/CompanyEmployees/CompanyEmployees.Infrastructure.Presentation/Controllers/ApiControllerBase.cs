using CompanyEmployees.Core.Domain.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace CompanyEmployees.Infrastructure.Presentation.Controllers;

public class ApiControllerBase : ControllerBase
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult ProcessError(OneOf<ApiNotFoundResponse, ApiBadRequestResponse> response)
    {
        var title = "An error occured.";

        return response.Value switch
        {
            ApiNotFoundResponse => NotFound(new ProblemDetails
            {
                Title = title,
                Detail = response.AsT0.Message,
                Status = StatusCodes.Status404NotFound,
                Type = response.AsT0.GetType().Name
            }),
            ApiBadRequestResponse => BadRequest(new ProblemDetails
            {
                Title = title,
                Detail = response.AsT1.Message,
                Status = StatusCodes.Status400BadRequest,
                Type = response.AsT1.GetType().Name
            }),
            _ => throw new NotImplementedException()
        };
    }
}