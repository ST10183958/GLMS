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


            var token = "YOUR_JWT_TOKEN";

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );
        }

   
        public async Task<List<Client>> GetClientsAsync()
        {
            var clients =
                await _httpClient.GetFromJsonAsync<List<Client>>(
                    "api/clients"
                );

            return clients ?? new List<Client>();
        }

        public async Task<Client?> GetClientByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Client>(
                $"api/clients/{id}"
            );
        }

   
        public async Task CreateClientAsync(Client client)
        {
            var response =
                await _httpClient.PostAsJsonAsync(
                    "api/clients",
                    client
                );

            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateClientAsync(int id, Client client)
        {
            var response =
                await _httpClient.PutAsJsonAsync(
                    $"api/clients/{id}",
                    client
                );

            response.EnsureSuccessStatusCode();
        }
        
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