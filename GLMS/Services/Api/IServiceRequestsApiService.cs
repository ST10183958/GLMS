using GLMS.Web.Models;

namespace GLMS.Web.Services
{
    public interface IServiceRequestsApiService
    {
        Task<List<ServiceRequest>>
            GetServiceRequestsAsync();

        Task<ServiceRequest?>
            GetServiceRequestByIdAsync(int id);

        Task CreateServiceRequestAsync(
            ServiceRequest request);

        Task UpdateServiceRequestAsync(
            int id,
            ServiceRequest request);

        Task DeleteServiceRequestAsync(int id);
    }
}