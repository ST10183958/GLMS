using System.Net.Http.Headers;
using System.Net.Http.Json;
using GLMS.Web.Models;

namespace GLMS.Web.Services
{
    public class ClientsApiService : IClientsApiService
    {
        private readonly HttpClient _httpClient;

        public ClientsApiService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("GLMSApi");

            // OPTIONAL JWT TOKEN
            // Replace with actual token retrieval later
            var token = "YOUR_JWT_TOKEN";

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );
        }

        // GET ALL CLIENTS
        public async Task<List<Client>> GetClientsAsync()
        {
            var clients =
                await _httpClient.GetFromJsonAsync<List<Client>>(
                    "api/clients"
                );

            return clients ?? new List<Client>();
        }

        // GET CLIENT BY ID
        public async Task<Client?> GetClientByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Client>(
                $"api/clients/{id}"
            );
        }

        // CREATE CLIENT
        public async Task CreateClientAsync(Client client)
        {
            var response =
                await _httpClient.PostAsJsonAsync(
                    "api/clients",
                    client
                );

            response.EnsureSuccessStatusCode();
        }

        // UPDATE CLIENT
        public async Task UpdateClientAsync(int id, Client client)
        {
            var response =
                await _httpClient.PutAsJsonAsync(
                    $"api/clients/{id}",
                    client
                );

            response.EnsureSuccessStatusCode();
        }

        // DELETE CLIENT
        public async Task DeleteClientAsync(int id)
        {
            var response =
                await _httpClient.DeleteAsync(
                    $"api/clients/{id}"
                );

            response.EnsureSuccessStatusCode();
        }
    }
}