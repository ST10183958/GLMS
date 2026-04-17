using System.Text.Json;

namespace GLMS.Web.Services
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
            var response = await _httpClient.GetAsync($"{baseUrl}USD");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var rates = doc.RootElement.GetProperty("rates");
            var zar = rates.GetProperty("ZAR").GetDecimal();

            return zar;
        }

        public decimal ConvertUsdToZar(decimal usdAmount, decimal rate)
        {
            return Math.Round(usdAmount * rate, 2);
        }
    }
}