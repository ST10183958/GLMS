using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using GLMS.Models;
using GLMS.Web.Enums;
using GLMS.Web.Models;
using GLMS.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        

        public async Task<IActionResult> Create()
        {
            var vm = new ContractCreateViewModel
            {
                Clients = await LoadClients()
            };

            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            ContractCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Clients = await LoadClients();
                return View(vm);
            }

            var token =
                HttpContext.Session.GetString("JWToken");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        token);
            }

            var contract = new Contract
            {
                ClientId = vm.ClientId,
                StartDate = vm.StartDate,
                EndDate = vm.EndDate,
                ServiceLevel = vm.ServiceLevel,
                Status = Enum.Parse<ContractStatus>(vm.Status)
            };

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

                vm.Clients = await LoadClients();

                return View(vm);
            }

            return RedirectToAction(nameof(Index));
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var token =
                HttpContext.Session.GetString("JWToken");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        token);
            }

            await _httpClient.DeleteAsync(
                $"api/contracts/{id}");

            return RedirectToAction(nameof(Index));
        }



        private async Task<List<SelectListItem>> LoadClients()
        {
            var response =
                await _httpClient.GetAsync(
                    "api/clients");

            if (!response.IsSuccessStatusCode)
            {
                return new List<SelectListItem>();
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

            return clients?
                .Select(c => new SelectListItem
                {
                    Value = c.ClientId.ToString(),
                    Text = c.Name
                })
                .ToList()
                ?? new List<SelectListItem>();
        }
    }
}