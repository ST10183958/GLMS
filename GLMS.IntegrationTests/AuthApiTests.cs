using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace GLMS.IntegrationTests
{
    public class AuthApiTests :
        IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuthApiTests(
            WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Login_ReturnsToken()
        {

            //Creating user to use for testing
            var registerData = new
            {
                username = "ST10183958",
                email = "ST10183958@gmail.com",
                password = "1234"
            };

            await _client.PostAsJsonAsync(
                "/api/auth/register",
                registerData
            );
            
           //Checking if user exists 
            var loginData = new
            {
                username = "ST10183958",
                password = "1234"
            };

            var response =
                await _client.PostAsJsonAsync(
                    "/api/auth/login",
                    loginData
                );

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);

            var json =
                await response.Content.ReadAsStringAsync();

            json.Should().Contain("token");
        }
    }
}