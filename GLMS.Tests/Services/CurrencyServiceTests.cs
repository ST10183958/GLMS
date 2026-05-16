using GLMS.Web.Services;

namespace GLMS.Tests
{
    public class CurrencyServiceTests
    {
        [Fact]
        public void ConvertUsdToZar_ShouldCalculateCorrectly()
        {
            var service = new CurrencyService(
                new HttpClient(),
                null!
            );

            var result = service.ConvertUsdToZar(100, 18);

            Assert.Equal(1800, result);
        }

        [Fact]
        public void ConvertUsdToZar_ZeroAmount_ShouldThrow()
        {
            var service = new CurrencyService(
                new HttpClient(),
                null!
            );

            Assert.Throws<ArgumentException>(() =>
                service.ConvertUsdToZar(0, 18));
        }

        [Fact]
        public void ConvertUsdToZar_NegativeAmount_ShouldThrow()
        {
            var service = new CurrencyService(
                new HttpClient(),
                null!
            );

            Assert.Throws<ArgumentException>(() =>
                service.ConvertUsdToZar(-10, 18));
        }

        [Fact]
        public void ConvertUsdToZar_InvalidRate_ShouldThrow()
        {
            var service = new CurrencyService(
                new HttpClient(),
                null!
            );

            Assert.Throws<ArgumentException>(() =>
                service.ConvertUsdToZar(100, 0));
        }
    }
}