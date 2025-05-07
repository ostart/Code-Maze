﻿using CompanyEmployees.Core.Services.Abstractions;
using CompanyEmployees.Infrastructure.Presentation.ActionFilters;
using CompanyEmployees.Infrastructure.Presentation.ModelBinders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.RateLimiting;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Infrastructure.Presentation.Controllers;

//[ApiVersion("1.0")]
[Route("api/companies")]
[ApiController]
//[ResponseCache(CacheProfileName = "120SecondsDuration")]
[OutputCache(PolicyName = "120SecondsDuration")]
[ApiExplorerSettings(GroupName = "v1")]
public class CompaniesController : ControllerBase
{
    private readonly IServiceManager _service;

    public CompaniesController(IServiceManager service) => _service = service;

    /// <summary>
    /// Gets the list of all companies
    /// </summary>
    /// <returns>The companies list</returns>
    [HttpGet(Name = "GetCompanies")]
    [EnableRateLimiting("SpecificPolicy")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetCompanies(CancellationToken ct)
    {
        var companies = await _service.CompanyService.GetAllCompaniesAsync(trackChanges: false, ct);

        return Ok(companies);
    }

    [HttpGet("{id:guid}", Name = "CompanyById")]
    //[ResponseCache(Duration = 60)]
    [OutputCache(Duration = 60)]
    [DisableRateLimiting]
    public async Task<IActionResult> GetCompany(Guid id, CancellationToken ct)
    {
        var company = await _service.CompanyService.GetCompanyAsync(id, trackChanges: false, ct);

        var etag = $"\"{Guid.NewGuid():n}\"";
        HttpContext.Response.Headers.ETag = etag;

        return Ok(company);
    }

    /// <summary>
    /// Creates a newly created company
    /// </summary>
    /// <param name="company"></param>
    /// <returns>A newly created company</returns>
    /// <response code="201">Returns the newly created item</response>
    /// <response code="400">If the item is null</response>
    /// <response code="422">If the model is invalid</response>
    [HttpPost(Name = "CreateCompany")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(422)]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company, 
        CancellationToken ct)
    {
        var createdCompany = await _service.CompanyService.CreateCompanyAsync(company, ct);

        return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
    }

    [HttpGet("collection/({ids})", Name = "CompanyCollection")]
    public async Task<IActionResult> GetCompanyCollection
        ([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids, CancellationToken ct)
    {
        var companies = await _service.CompanyService.GetByIdsAsync(ids, trackChanges: false, ct);

        return Ok(companies);
    }

    [HttpPost("collection")]
    public async Task<IActionResult> CreateCompanyCollection
        ([FromBody] IEnumerable<CompanyForCreationDto> companyCollection, CancellationToken ct)
    {
        var result = await _service.CompanyService.CreateCompanyCollectionAsync(companyCollection, ct);

        return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCompany(Guid id, CancellationToken ct)
    {
        await _service.CompanyService.DeleteCompanyAsync(id, trackChanges: false, ct);

        return NoContent();
    }

    [HttpPut("{id:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company, CancellationToken ct)
    {
        await _service.CompanyService.UpdateCompanyAsync(id, company, trackChanges: true, ct);

        return NoContent();
    }

    [HttpOptions]
    public IActionResult GetCompaniesOptions()
    {
        Response.Headers.Add("Allow", "GET, OPTIONS, POST, PUT, DELETE");

        return Ok();
    }
}