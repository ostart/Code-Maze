﻿using CompanyEmployees.Core.Domain.Entities;
using CompanyEmployees.IntegrationTests.Factories;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace CompanyEmployees.IntegrationTests;

public class SortingPagingFilertingSearchingTests : IClassFixture<CompanyEmployeesTestcontainersFactory>
{
	private readonly HttpClient _client;
	private const string EmployeesUrl = "/api/companies";

	public SortingPagingFilertingSearchingTests(CompanyEmployeesTestcontainersFactory factory)
	{
		_client = factory.CreateClient();
	}

	[Fact]
	public async Task WhenGetEndpointsRequested_ThenReturnsOKAndEntites()
	{
		// Arrange
		_client.DefaultRequestHeaders.Add("Accept", "application/vnd.codemaze.apiroot+json");

		// Act
		var response = await _client.GetAsync($"{EmployeesUrl}/C9D4C053-49B6-410C-BC78-2D54A9991870/employees");

		// Assert
		response.EnsureSuccessStatusCode(); // Status Code 200-299

		var result = await response.Content.ReadFromJsonAsync<List<Entity>>();
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal(2, result?.Count);
		Assert.True(response.Headers.Contains("X-Pagination"));
	}

	[Fact]
	public async Task WhenPaging_ThenReturnsOKAndEntity()
	{
		// Arrange
		_client.DefaultRequestHeaders.Add("Accept", "application/vnd.codemaze.apiroot+json");

		var url = $"{EmployeesUrl}/C9D4C053-49B6-410C-BC78-2D54A9991870/employees";
		var queryParams = new Dictionary<string, string>
		{
			["pageNumber"] = "2",
			["pageSize"] = "1",
		};
		var urlWithParams = QueryHelpers.AddQueryString(url, queryParams);

		// Act
		var response = await _client.GetAsync(urlWithParams);

		// Assert
		response.EnsureSuccessStatusCode(); // Status Code 200-299

		var result = await response.Content.ReadFromJsonAsync<List<Entity>>();
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal(1, result?.Count);
		Assert.True(response.Headers.Contains("X-Pagination"));
	}

	[Fact]
	public async Task WhenFiltering_ThenReturnsOKAndEntity()
	{
		// Act
		_client.DefaultRequestHeaders.Add("Accept", "application/vnd.codemaze.apiroot+json");

		var url = $"{EmployeesUrl}/C9D4C053-49B6-410C-BC78-2D54A9991870/employees";
		var queryParams = new Dictionary<string, string>
		{
			["minAge"] = "29",
			["maxAge"] = "31",
		};
		var urlWithParams = QueryHelpers.AddQueryString(url, queryParams);
		var response = await _client.GetAsync(urlWithParams);

		// Assert
		response.EnsureSuccessStatusCode(); // Status Code 200-299

		var result = await response.Content.ReadFromJsonAsync<List<Entity>>();
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal(1, result?.Count);
		Assert.True(response.Headers.Contains("X-Pagination"));
	}


	[Fact]
	public async Task WhenSearching_ThenReturnsOKAndEntity()
	{
		// Arrange
		_client.DefaultRequestHeaders.Add("Accept", "application/vnd.codemaze.apiroot+json");

		var url = $"{EmployeesUrl}/C9D4C053-49B6-410C-BC78-2D54A9991870/employees";
		var queryParams = new Dictionary<string, string>
		{
			["searchTerm"] = "McLeaf"
		};
		var urlWithParams = QueryHelpers.AddQueryString(url, queryParams);

		// Act
		var response = await _client.GetAsync(urlWithParams);

		// Assert
		response.EnsureSuccessStatusCode(); // Status Code 200-299

		var result = await response.Content.ReadFromJsonAsync<List<Entity>>();
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal(1, result?.Count);
		Assert.True(response.Headers.Contains("X-Pagination"));
	}

	[Fact]
	public async Task WhenSorting_ThenReturnsOKAndSortedEntitesByAge()
	{
		// Arrange
		_client.DefaultRequestHeaders.Add("Accept", "application/vnd.codemaze.apiroot+json");

		var url = $"{EmployeesUrl}/C9D4C053-49B6-410C-BC78-2D54A9991870/employees";
		var queryParams = new Dictionary<string, string>
		{
			["orderBy"] = "age desc"
		};
		var urlWithParams = QueryHelpers.AddQueryString(url, queryParams);

		// Act
		var response = await _client.GetAsync(urlWithParams);

		// Assert
		response.EnsureSuccessStatusCode(); // Status Code 200-299

		var result = await response.Content.ReadFromJsonAsync<List<Entity>>();
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal(2, result?.Count);
		result.First().TryGetValue("Age", out object ageFirst);
		result.Last().TryGetValue("Age", out object ageSecond);
		Assert.True(Convert.ToInt32(ageFirst.ToString()) > Convert.ToInt32(ageSecond.ToString()));
		Assert.True(response.Headers.Contains("X-Pagination"));
	}
}
