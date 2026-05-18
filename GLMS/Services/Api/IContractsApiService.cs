using GLMS.Web.Models;

namespace GLMS.Web.Services;

public interface IContractsApiService
{
    Task<List<Contract>> GetContractsAsync();

    Task CreateContractAsync(Contract contract);

    Task DeleteContractAsync(int id);
}