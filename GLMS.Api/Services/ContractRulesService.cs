using GLMS.Api.Enums;
using GLMS.Api.Models;

namespace GLMS.Api.Services
{
    public class ContractRulesService : IContractRulesService
    {
        public bool CanCreateServiceRequest(Contract contract)
        {
            return contract.Status != ContractStatus.Expired
                   && contract.Status != ContractStatus.OnHold;
        }
    }
}