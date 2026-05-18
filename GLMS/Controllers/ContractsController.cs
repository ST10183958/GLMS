using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using GLMS.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GLMS.Web.Controllers
{
    public class ContractsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ContractsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // =========================
        // INDEX
        // =========================

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var client = _httpClientFactory.CreateClient("GLMSApi");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("api/contracts");

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<Contract>());
            }

            var json = await response.Content.ReadAsStringAsync();

            var contracts = JsonSerializer.Deserialize<List<Contract>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return View(contracts);
        }

        // =========================
        // DETAILS
        // =========================

        public async Task<IActionResult> Details(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var client = _httpClientFactory.CreateClient("GLMSApi");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"api/contracts/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var json = await response.Content.ReadAsStringAsync();

            var contract = JsonSerializer.Deserialize<Contract>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return View(contract);
        }

        // =========================
        // CREATE GET
        // =========================

        public IActionResult Create()
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        // =========================
        // CREATE POST
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contract contract)
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                return View(contract);
            }

            var client = _httpClientFactory.CreateClient("GLMSApi");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(contract);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync(
                "api/contracts",
                content
            );

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    "",
                    "Failed to create contract."
                );

                return View(contract);
            }

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // DELETE GET
        // =========================

        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var client = _httpClientFactory.CreateClient("GLMSApi");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"api/contracts/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var json = await response.Content.ReadAsStringAsync();

            var contract = JsonSerializer.Deserialize<Contract>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return View(contract);
        }

        // =========================
        // DELETE POST
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int contractId)
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var client = _httpClientFactory.CreateClient("GLMSApi");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            await client.DeleteAsync($"api/contracts/{contractId}");

            return RedirectToAction(nameof(Index));
        }
    }
}