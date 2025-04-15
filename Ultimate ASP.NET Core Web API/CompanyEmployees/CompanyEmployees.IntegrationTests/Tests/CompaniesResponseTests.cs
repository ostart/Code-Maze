using System.Net;
using System.Net.Http.Json;
using CompanyEmployees.IntegrationTests.Factories;
using Shared.DataTransferObjects;

namespace CompanyEmployees.IntegrationTests.Tests;

public class CompaniesResponseTests : IClassFixture<CompanyEmployeesTestcontainersFactory>
{
    private readonly HttpClient _client;
    private const string CompaniesUrl = "/api/companies";

    public CompaniesResponseTests(CompanyEmployeesTestcontainersFactory factory)
    {
        _client = factory.CreateClient();
    }
    
    [Theory]
    [InlineData($"{CompaniesUrl}/3d490a70-94ce-4d15-9494-5248280c2ce3")]
    [InlineData($"{CompaniesUrl}/c9d4c053-49b6-410c-bc78-2d54a9991870")]
    public async Task WhenGetEndpointsRequested_ThenReturnsOKStatusCode(string url)
    {
        // Act
        var response = await _client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<CompanyDto>();
        Assert.IsType<CompanyDto>(result);
        Assert.False(string.IsNullOrEmpty(result.Name));
        Assert.False(string.IsNullOrEmpty(result.FullAddress));
    }
    
    [Fact]
    public async Task WhenCreateNewEntityRequested_ThenReturns201Created()
    {
        // Arrange
        var company = new CompanyForCreationDto
        {
            Name = "Test",
            Address = "TestAddress",
            Country = "USA"
        };

        // Act
        var response = await _client.PostAsJsonAsync(CompaniesUrl, company);
        var location = response.Headers.Location;

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(location);
    }
    
    [Fact]
    public async Task WhenCreateNewEntityRequested_ThenReturnsNewCompany()
    {
        // Arrange
        var company = new CompanyForCreationDto
        {
            Name = "Test",
            Address = "TestAddress",
            Country = "USA"
        };

        // Act
        var response = await _client.PostAsJsonAsync(CompaniesUrl, company);
        var location = response.Headers.Location;

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(location);
        
        // Act
        var response2 = await _client.GetAsync(location);

        // Assert
        response2.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8", response2.Content.Headers.ContentType?.ToString());
        Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
        var result = await response2.Content.ReadFromJsonAsync<CompanyDto>();
        Assert.IsType<CompanyDto>(result);
        Assert.False(string.IsNullOrEmpty(result.Name));
        Assert.False(string.IsNullOrEmpty(result.FullAddress));
    }
    
    [Fact]
    public async Task WhenUpdateRequested_ThenReturns204NoContent()
    {
        // Arrange
        var company = new CompanyForUpdateDto
        {
            Name = "Test",
            Address = "TestAddress",
            Country = "USA"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"{CompaniesUrl}/c9d4c053-49b6-410c-bc78-2d54a9991870", company);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task WhenEntityDelete_ThenReturns204NoContent()
    {
        // Arrange
        var company = new CompanyForCreationDto
        {
            Name = "Test2",
            Address = "TestAddress2",
            Country = "USA"
        };

        // Act
        var responseNewCompany = await _client.PostAsJsonAsync(CompaniesUrl, company);
        var companyId = responseNewCompany.Headers?.Location?.Segments.LastOrDefault();

        var responseDelete = await _client.DeleteAsync($"{CompaniesUrl}/{companyId}");

        // Assert
        responseDelete.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal(HttpStatusCode.NoContent, responseDelete.StatusCode);
    }
}