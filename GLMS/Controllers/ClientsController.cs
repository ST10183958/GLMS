using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using GLMS.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GLMS.Web.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ClientsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

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

            var response = await client.GetAsync("api/clients");

            var json = await response.Content.ReadAsStringAsync();

            var clients = JsonSerializer.Deserialize<List<Client>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return View(clients);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client model)
        {
            var token = HttpContext.Session.GetString("JWToken");

            var client = _httpClientFactory.CreateClient("GLMSApi");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(model);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            await client.PostAsync("api/clients", content);

            return RedirectToAction(nameof(Index));
        }
    }
}