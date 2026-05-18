using System.Net.Http.Json;
using GLMS.Web.Models;

namespace GLMS.Web.Services;

public class ContractsApiService : IContractsApiService
{
    private readonly HttpClient _httpClient;

    public ContractsApiService(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("GLMSApi");
    }

    public async Task<List<Contract>> GetContractsAsync()
    {
        return await _httpClient
                   .GetFromJsonAsync<List<Contract>>(
                       "api/contracts")
               ?? new List<Contract>();
    }

    public async Task CreateContractAsync(Contract contract)
    {
        var response = await _httpClient
            .PostAsJsonAsync("api/contracts", contract);

        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteContractAsync(int id)
    {
        var response =
            await _httpClient.DeleteAsync(
                $"api/contracts/{id}");

        response.EnsureSuccessStatusCode();
    }
}