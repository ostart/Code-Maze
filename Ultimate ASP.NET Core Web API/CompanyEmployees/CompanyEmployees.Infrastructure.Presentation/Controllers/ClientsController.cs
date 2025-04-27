using CompanyEmployees.Core.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Infrastructure.Presentation.Controllers;

[Route("api/clients")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly IServiceManager _service;

    public ClientsController(IServiceManager service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetClients(CancellationToken ct)
    {
        var clients = await _service.ClientService.GetAllClientsAsync(trackChanges: false, ct);

        return Ok(clients);
    }

    [HttpPost]
    public async Task<IActionResult> CreateClient([FromBody] ClientForCreationDto client,
        CancellationToken ct)
    {
        if (client is null)
            return BadRequest("ClientForCreation object is null");

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var createdClient = await _service.ClientService.CreateClientAsync(client, ct);

        return Created("URI for GetCompanyById", createdClient);
    }
}