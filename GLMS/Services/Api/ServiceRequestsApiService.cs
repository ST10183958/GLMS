using System.Net.Http.Json;
using GLMS.Web.Models;

namespace GLMS.Web.Services
{
    public class ServiceRequestsApiService
        : IServiceRequestsApiService
    {
        private readonly HttpClient _httpClient;

        public ServiceRequestsApiService(
            IHttpClientFactory factory)
        {
            _httpClient =
                factory.CreateClient("GLMSApi");
        }

        public async Task<List<ServiceRequest>>
            GetServiceRequestsAsync()
        {
            return await _httpClient
                       .GetFromJsonAsync<
                           List<ServiceRequest>>(
                           "api/servicerequests")
                   ?? new List<ServiceRequest>();
        }

        public async Task<ServiceRequest?>
            GetServiceRequestByIdAsync(int id)
        {
            return await _httpClient
                .GetFromJsonAsync<ServiceRequest>(
                    $"api/servicerequests/{id}");
        }

        public async Task CreateServiceRequestAsync(
            ServiceRequest request)
        {
            var response =
                await _httpClient.PostAsJsonAsync(
                    "api/servicerequests",
                    request);

            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateServiceRequestAsync(
            int id,
            ServiceRequest request)
        {
            var response =
                await _httpClient.PutAsJsonAsync(
                    $"api/servicerequests/{id}",
                    request);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteServiceRequestAsync(
            int id)
        {
            var response =
                await _httpClient.DeleteAsync(
                    $"api/servicerequests/{id}");

            response.EnsureSuccessStatusCode();
        }
    }
}