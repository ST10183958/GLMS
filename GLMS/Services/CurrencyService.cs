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

            _httpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        public async Task<decimal> GetUsdToZarRateAsync()
        {
            try
            {
                var baseUrl = _configuration["CurrencyApi:BaseUrl"];

                if (string.IsNullOrWhiteSpace(baseUrl))
                {
                    throw new InvalidOperationException(
                        "Currency API base URL is not configured."
                    );
                }

                var response = await _httpClient.GetAsync($"{baseUrl}USD");

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(json);

                var result = doc.RootElement
                    .GetProperty("result")
                    .GetString();

                if (result != "success")
                {
                    var errorType = doc.RootElement.TryGetProperty(
                        "error-type",
                        out var errorProp)
                        ? errorProp.GetString()
                        : "unknown";

                    throw new InvalidOperationException(
                        $"Currency API returned error: {errorType}"
                    );
                }

                var rates = doc.RootElement
                    .GetProperty("conversion_rates");

                var zar = rates
                    .GetProperty("ZAR")
                    .GetDecimal();

                if (zar <= 0)
                {
                    throw new InvalidOperationException(
                        "Invalid exchange rate returned from API."
                    );
                }

                return zar;
            }
            catch (TaskCanceledException)
            {
                throw new Exception(
                    "Currency API request timed out."
                );
            }
            catch (HttpRequestException)
            {
                throw new Exception(
                    "Currency API is currently unavailable."
                );
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Currency conversion failed: {ex.Message}"
                );
            }
        }

        public decimal ConvertUsdToZar(decimal usdAmount, decimal rate)
        {
            if (usdAmount <= 0)
            {
                throw new ArgumentException(
                    "USD amount must be greater than zero."
                );
            }

            if (rate <= 0)
            {
                throw new ArgumentException(
                    "Exchange rate must be greater than zero."
                );
            }

            return Math.Round(usdAmount * rate, 2);
        }
    }
}