using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace GLMS.IntegrationTests
{
    public class ContractsApiTests :
        IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ContractsApiTests(
            WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetContracts_Returns200Ok()
        {

            var response =
                await _client.GetAsync("/api/contracts");
            
            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetContracts_ReturnsJsonData()
        {

            var response =
                await _client.GetAsync("/api/contracts");
            
            response.EnsureSuccessStatusCode();

            var json =
                await response.Content.ReadAsStringAsync();

            json.Should().NotBeNullOrWhiteSpace();
        }
    }
}