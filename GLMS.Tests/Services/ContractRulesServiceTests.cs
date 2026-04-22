using GLMS.Web.Enums;
using GLMS.Web.Models;
using GLMS.Web.Services;
using Xunit;

namespace GLMS.Tests.Services
{
    public class ContractRulesServiceTests
    {
        [Fact]
        public void ShouldBlock_WhenExpired()
        {
            var service = new ContractRulesService();

            var contract = new Contract
            {
                Status = ContractStatus.Expired
            };

            var result = service.CanCreateServiceRequest(contract);

            Assert.False(result);
        }

        [Fact]
        public void ShouldBlock_WhenOnHold()
        {
            var service = new ContractRulesService();

            var contract = new Contract
            {
                Status = ContractStatus.OnHold
            };

            var result = service.CanCreateServiceRequest(contract);

            Assert.False(result);
        }

        [Fact]
        public void ShouldAllow_WhenActive()
        {
            var service = new ContractRulesService();

            var contract = new Contract
            {
                Status = ContractStatus.Active
            };

            var result = service.CanCreateServiceRequest(contract);

            Assert.True(result);
        }
    }
}