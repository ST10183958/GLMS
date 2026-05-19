using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace GLMS.IntegrationTests
{
    public class ClientsApiTests :
        IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ClientsApiTests(
            WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetClients_ReturnsOk()
        {
            var response =
                await _client.GetAsync("/api/clients");

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);
        }
    }
}