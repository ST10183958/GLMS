using GLMS.Web.Models;
using GLMS.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GLMS.Web.Controllers;


public class ContractsController : Controller
{
    private readonly IContractsApiService _api;

    public ContractsController(
        IContractsApiService api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        var contracts =
            await _api.GetContractsAsync();

        return View(contracts);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        Contract contract)
    {
        if (!ModelState.IsValid)
            return View(contract);

        await _api.CreateContractAsync(contract);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        await _api.DeleteContractAsync(id);

        return RedirectToAction(nameof(Index));
    }
}