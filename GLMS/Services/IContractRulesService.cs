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
            if (contract.Status == ContractStatus.Expired)
                return false;

            if (contract.Status == ContractStatus.OnHold)
                return false;

            return true;
        }
    }
}