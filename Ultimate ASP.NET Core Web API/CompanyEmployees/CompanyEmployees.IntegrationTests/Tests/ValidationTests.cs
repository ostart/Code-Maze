using System.Net;
using System.Net.Http.Json;
using CompanyEmployees.IntegrationTests.Factories;
using Shared.DataTransferObjects;

namespace CompanyEmployees.IntegrationTests.Tests;

public class ValidationTests : IClassFixture<CompanyEmployeesTestcontainersFactory>
{
    private readonly HttpClient _client;
    private const string CompaniesUrl = "/api/companies";

    public ValidationTests(CompanyEmployeesTestcontainersFactory factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task WhenEntityDoesntExist_ThenReturns404NotFound()
    {
        // Act
        var response = await _client.GetAsync($"{CompaniesUrl}/d9d4c053-49b6-410c-bc78-2d54a9991870");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task WhenModelStateInvalidOnCreation_ThenReturns422UnprocessableEntity()
    {
        // Arrange
        var company = new CompanyForCreationDto
        {
            Address = "TestAddress",
            Country = "USA"
        };

        // Act
        var response = await _client.PostAsJsonAsync(CompaniesUrl, company);

        // Assert
        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task WhenEntityNullOnCreation_ThenReturns400BadRequest()
    {
        // Arrange
        CompanyForCreationDto? companyDto = null;

        // Act
        var response = await _client.PostAsJsonAsync(CompaniesUrl, companyDto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task WhenEntityNullOnCreation_ThenReturns415UnsupportedMediaType()
    {
        // Act
        var response = await _client.PostAsync(CompaniesUrl, null);

        // Assert
        Assert.Equal(HttpStatusCode.UnsupportedMediaType, response.StatusCode);
    }

    [Fact]
    public async Task WhenGetAllRequested_ThenReturns401Unauthorized()
    {
        // Act
        var response = await _client.GetAsync(CompaniesUrl);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}