using GLMS.Web.Services;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace GLMS.Tests.Services
{
    public class CurrencyServiceTests
    {
        [Fact]
        public void ConvertUsdToZar_ShouldReturnCorrectValue()
        {
            // Arrange
            var config = new ConfigurationBuilder().Build();
            var service = new CurrencyService(new HttpClient(), config);

            decimal usd = 100m;
            decimal rate = 18.50m;

            // Act
            var result = service.ConvertUsdToZar(usd, rate);

            // Assert
            Assert.Equal(1850.00m, result);
        }
    }
}