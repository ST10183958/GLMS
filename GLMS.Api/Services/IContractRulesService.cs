using GLMS.Api.Models;

namespace GLMS.Api.Services
{
    public interface IContractRulesService
    {
        bool CanCreateServiceRequest(Contract contract);
    }
}