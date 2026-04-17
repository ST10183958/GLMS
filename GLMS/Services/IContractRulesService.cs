using GLMS.Web.Enums;
using GLMS.Web.Models;

namespace GLMS.Web.Services
{
    public interface IContractRulesService
    {
        bool CanCreateServiceRequest(Contract contract);
    }

    public class ContractRulesService : IContractRulesService
    {
        public bool CanCreateServiceRequest(Contract contract)
        {
            return contract.Status != ContractStatus.Expired &&
                   contract.Status != ContractStatus.OnHold;
        }
    }
}