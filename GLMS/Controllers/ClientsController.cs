using System.Text;
using System.Text.Json;
using GLMS.Models;
using GLMS.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GLMS.Controllers
{
    public class ClientsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ClientsController(
            IHttpClientFactory factory,
            IConfiguration configuration)
        {
            _httpClient = factory.CreateClient();

            _configuration = configuration;

            _httpClient.BaseAddress =
                new Uri(_configuration["ApiSettings:BaseUrl"]!);
        }

        // =========================================
        // GET CLIENTS
        // =========================================

        public async Task<IActionResult> Index()
        {
            var response =
                await _httpClient.GetAsync("api/clients");

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<Client>());
            }

            var json =
                await response.Content.ReadAsStringAsync();

            var clients =
                JsonSerializer.Deserialize<List<Client>>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            return View(clients);
        }

        // =========================================
        // CREATE PAGE
        // =========================================

        public IActionResult Create()
        {
            return View();
        }

        // =========================================
        // CREATE CLIENT
        // =========================================

        [HttpPost]
        public async Task<IActionResult> Create(Client client)
        {
            var json =
                JsonSerializer.Serialize(client);

            var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

            var response =
                await _httpClient.PostAsync(
                    "api/clients",
                    content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    "",
                    "Failed to create client.");

                return View(client);
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
                    $"api/clients/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var json =
                await response.Content.ReadAsStringAsync();

            var client =
                JsonSerializer.Deserialize<Client>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            return View(client);
        }
    }
}