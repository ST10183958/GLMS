using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using GLMS.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GLMS.Web.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ServiceRequestsController(IHttpClientFactory httpClientFactory)
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

            var response = await client.GetAsync(
                "api/servicerequests"
            );

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<ServiceRequest>());
            }

            var json = await response.Content.ReadAsStringAsync();

            var requests =
                JsonSerializer.Deserialize<List<ServiceRequest>>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            return View(requests);
        }

        // =========================
        // CREATE GET
        // =========================

        public IActionResult Create()
        {
            return View();
        }

        // =========================
        // CREATE POST
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            ServiceRequest request
        )
        {
            var token = HttpContext.Session.GetString("JWToken");

            var client = _httpClientFactory.CreateClient("GLMSApi");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(request);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync(
                "api/servicerequests",
                content
            );

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    "",
                    "Failed to create service request."
                );

                return View(request);
            }

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // DELETE
        // =========================

        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");

            var client = _httpClientFactory.CreateClient("GLMSApi");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(
                $"api/servicerequests/{id}"
            );

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var json = await response.Content.ReadAsStringAsync();

            var request =
                JsonSerializer.Deserialize<ServiceRequest>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(
            int serviceRequestId
        )
        {
            var token = HttpContext.Session.GetString("JWToken");

            var client = _httpClientFactory.CreateClient("GLMSApi");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            await client.DeleteAsync(
                $"api/servicerequests/{serviceRequestId}"
            );

            return RedirectToAction(nameof(Index));
        }
    }
}