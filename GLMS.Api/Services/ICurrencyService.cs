namespace GLMS.Api.Services
{
    public interface ICurrencyService
    {
        Task<decimal> GetUsdToZarRateAsync();

        decimal ConvertUsdToZar(decimal usdAmount, decimal rate);
    }
}