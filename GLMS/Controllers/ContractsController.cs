using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using GLMS.Models;
using GLMS.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GLMS.Controllers
{
    public class ContractsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ContractsController(
            IHttpClientFactory factory,
            IConfiguration configuration)
        {
            _httpClient = factory.CreateClient();
            _configuration = configuration;

            _httpClient.BaseAddress =
                new Uri(_configuration["ApiSettings:BaseUrl"]!);
        }

        // =========================================
        // GET ALL CONTRACTS
        // =========================================

        public async Task<IActionResult> Index(
            string? status,
            DateTime? startDate,
            DateTime? endDate)
        {
            var url =
                $"api/contracts?status={status}&startDate={startDate}&endDate={endDate}";

            var response =
                await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Failed to load contracts.";
                return View(new List<Contract>());
            }

            var json =
                await response.Content.ReadAsStringAsync();

            var contracts =
                JsonSerializer.Deserialize<List<Contract>>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            return View(contracts);
        }

        // =========================================
        // CREATE PAGE
        // =========================================

        public IActionResult Create()
        {
            return View();
        }

        // =========================================
        // CREATE CONTRACT
        // =========================================

        [HttpPost]
        public async Task<IActionResult> Create(Contract contract)
        {
            var token = HttpContext.Session.GetString("JWToken");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var json =
                JsonSerializer.Serialize(contract);

            var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

            var response =
                await _httpClient.PostAsync(
                    "api/contracts",
                    content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    "",
                    "Failed to create contract.");

                return View(contract);
            }

            return RedirectToAction(nameof(Index));
        }

        // =========================================
        // DETAILS
        // =========================================

        public async Task<IActionResult> Details(int id)
        {
            var response =
                await _httpClient.GetAsync(
                    $"api/contracts/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var json =
                await response.Content.ReadAsStringAsync();

            var contract =
                JsonSerializer.Deserialize<Contract>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            return View(contract);
        }

        // =========================================
        // DELETE PAGE
        // =========================================

        public async Task<IActionResult> Delete(int id)
        {
            var response =
                await _httpClient.GetAsync(
                    $"api/contracts/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var json =
                await response.Content.ReadAsStringAsync();

            var contract =
                JsonSerializer.Deserialize<Contract>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            return View(contract);
        }

        // =========================================
        // DELETE CONFIRMED
        // =========================================

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response =
                await _httpClient.DeleteAsync(
                    $"api/contracts/{id}");

            return RedirectToAction(nameof(Index));
        }
    }
}