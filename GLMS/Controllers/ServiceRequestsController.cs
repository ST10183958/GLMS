using GLMS.Web.Data;
using GLMS.Web.Models;
using GLMS.Web.Services;
using GLMS.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Web.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrencyService _currencyService;
        private readonly IContractRulesService _contractRulesService;

        public ServiceRequestsController(
            ApplicationDbContext context,
            ICurrencyService currencyService,
            IContractRulesService contractRulesService)
        {
            _context = context;
            _currencyService = currencyService;
            _contractRulesService = contractRulesService;
        }

        public async Task<IActionResult> Index()
        {
            var serviceRequests = await _context.ServiceRequests
                .Include(sr => sr.Contract)
                .ToListAsync();

            return View(serviceRequests);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new ServiceRequestCreateViewModel
            {
                Contracts = await _context.Contracts
                    .Include(c => c.Client)
                    .Select(c => new SelectListItem
                    {
                        Value = c.ContractId.ToString(),
                        Text = $"{c.Client!.Name} - Contract #{c.ContractId} ({c.Status})"
                    })
                    .ToListAsync()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequestCreateViewModel vm)
        {
            var contract = await _context.Contracts.FindAsync(vm.ContractId);

            if (contract == null)
                ModelState.AddModelError("ContractId", "Selected contract was not found.");
            else if (!_contractRulesService.CanCreateServiceRequest(contract))
                ModelState.AddModelError("ContractId", "Cannot create a service request for an Expired or On Hold contract.");

            if (!ModelState.IsValid)
            {
                vm.Contracts = await _context.Contracts
                    .Include(c => c.Client)
                    .Select(c => new SelectListItem
                    {
                        Value = c.ContractId.ToString(),
                        Text = $"{c.Client!.Name} - Contract #{c.ContractId} ({c.Status})"
                    })
                    .ToListAsync();

                return View(vm);
            }

            var rate = await _currencyService.GetUsdToZarRateAsync();
            var costZar = _currencyService.ConvertUsdToZar(vm.CostUsd, rate);

            var entity = new ServiceRequest
            {
                ContractId = vm.ContractId,
                Description = vm.Description,
                CostUsd = vm.CostUsd,
                ExchangeRateUsed = rate,
                CostZar = costZar,
                Status = vm.Status
            };

            _context.ServiceRequests.Add(entity);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetExchangeRate()
        {
            var rate = await _currencyService.GetUsdToZarRateAsync();
            return Json(new { rate });
        }
    }
}