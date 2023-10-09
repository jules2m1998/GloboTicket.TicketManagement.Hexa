using GloboTicket.TicketManagement.Api.IntegrationTests.Base;
using GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesList;
using GloboTicket.TicketManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Api.IntegrationTests.Controllers;

public class CategoryControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public CategoryControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true,
        });
    }

    [Fact]
    public async Task ReturnsSuccessResult()
    {
        var response = await _client.GetAsync("/api/category/all");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<CategoryListVm>>(responseString);

        result.ShouldBeOfType<List<CategoryListVm>>();
        result.ShouldNotBeNull();
        result.Count.ShouldBe(4);
    }
}
