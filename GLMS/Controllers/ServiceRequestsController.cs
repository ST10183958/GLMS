using System.Text;
using System.Text.Json;
using GLMS.Models;
using GLMS.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GLMS.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ServiceRequestsController(
            IHttpClientFactory factory,
            IConfiguration configuration)
        {
            _httpClient = factory.CreateClient();

            _configuration = configuration;

            _httpClient.BaseAddress =
                new Uri(_configuration["ApiSettings:BaseUrl"]!);
        }

        // =========================================
        // GET ALL
        // =========================================

        public async Task<IActionResult> Index()
        {
            var response =
                await _httpClient.GetAsync(
                    "api/servicerequests");

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<ServiceRequest>());
            }

            var json =
                await response.Content.ReadAsStringAsync();

            var requests =
                JsonSerializer.Deserialize<List<ServiceRequest>>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            return View(requests);
        }

        // =========================================
        // CREATE PAGE
        // =========================================

        public IActionResult Create()
        {
            return View();
        }

        // =========================================
        // CREATE
        // =========================================

        [HttpPost]
        public async Task<IActionResult> Create(
            ServiceRequest request)
        {
            var json =
                JsonSerializer.Serialize(request);

            var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

            var response =
                await _httpClient.PostAsync(
                    "api/servicerequests",
                    content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    "",
                    "Failed to create service request.");

                return View(request);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}