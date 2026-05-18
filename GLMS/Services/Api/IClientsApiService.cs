using GLMS.Web.Models;

namespace GLMS.Web.Services
{
    public interface IClientsApiService
    {
        Task<List<Client>> GetClientsAsync();

        Task<Client?> GetClientByIdAsync(int id);

        Task CreateClientAsync(Client client);

        Task UpdateClientAsync(int id, Client client);

        Task DeleteClientAsync(int id);
    }
}