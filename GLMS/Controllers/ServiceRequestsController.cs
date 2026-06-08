using System.Text;
using System.Text.Json;
using GLMS.Web.Models;
using GLMS.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLMS.Controllers;

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

    public async Task<IActionResult> Index()
    {
        var response =
            await _httpClient.GetAsync("api/servicerequests");

        if (!response.IsSuccessStatusCode)
            return View(new List<ServiceRequest>());

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

    public async Task<IActionResult> Create()
    {
        var vm = new ServiceRequestCreateViewModel
        {
            Contracts = await LoadContracts()
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        ServiceRequestCreateViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Contracts = await LoadContracts();
            return View(vm);
        }

        var request = new ServiceRequest
        {
            ContractId = vm.ContractId,
            Description = vm.Description,
            CostUsd = vm.CostUsd,
            Status = vm.Status
        };

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
            var error =
                await response.Content.ReadAsStringAsync();

            ModelState.AddModelError(
                "",
                error);

            vm.Contracts = await LoadContracts();

            return View(vm);
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task<List<SelectListItem>> LoadContracts()
    {
        var response =
            await _httpClient.GetAsync("api/contracts");

        if (!response.IsSuccessStatusCode)
            return new List<SelectListItem>();

        var json =
            await response.Content.ReadAsStringAsync();

        var contracts =
            JsonSerializer.Deserialize<List<Contract>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

        return contracts?
            .Select(c => new SelectListItem
            {
                Value = c.ContractId.ToString(),
                Text =
                    $"Contract #{c.ContractId} - {c.Client?.Name}"
            })
            .ToList()
            ?? new List<SelectListItem>();
    }
}