using System.Text.Json;

namespace GLMS.Api.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CurrencyService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<decimal> GetUsdToZarRateAsync()
        {
            var baseUrl = _configuration["CurrencyApi:BaseUrl"];

            var response = await _httpClient.GetAsync(baseUrl + "USD");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);

            var rate = doc.RootElement
                .GetProperty("conversion_rates")
                .GetProperty("ZAR")
                .GetDecimal();

            return rate;
        }

        public decimal ConvertUsdToZar(decimal usdAmount, decimal rate)
        {
            if (usdAmount <= 0)
                throw new ArgumentException("USD amount must be greater than zero.");

            if (rate <= 0)
                throw new ArgumentException("Exchange rate must be greater than zero.");

            return Math.Round(usdAmount * rate, 2);
        }
    }
}