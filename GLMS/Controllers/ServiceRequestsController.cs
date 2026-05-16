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
            var requests = await _context.ServiceRequests
                .Include(s => s.Contract)
                .ToListAsync();

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
        public async Task<IActionResult> Create(ServiceRequestCreateViewModel vm)
        {
            if (vm.CostUsd <= 0)
            {
                ModelState.AddModelError("CostUsd",
                    "USD amount must be greater than zero.");
            }

            var contract = await _context.Contracts.FindAsync(vm.ContractId);

            if (contract == null)
            {
                ModelState.AddModelError("ContractId",
                    "Selected contract not found.");
            }
            else if (!_contractRulesService.CanCreateServiceRequest(contract))
            {
                ModelState.AddModelError("ContractId",
                    "Cannot create service request for Expired or On Hold contracts.");
            }

            if (!ModelState.IsValid)
            {
                vm.Contracts = await LoadContracts();
                return View(vm);
            }

            decimal rate;
            decimal costZar;

            try
            {
                rate = await _currencyService.GetUsdToZarRateAsync();

                costZar = _currencyService.ConvertUsdToZar(
                    vm.CostUsd,
                    rate
                );
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                vm.Contracts = await LoadContracts();

                return View(vm);
            }

            var request = new ServiceRequest
            {
                ContractId = vm.ContractId,
                Description = vm.Description,
                CostUsd = vm.CostUsd,
                ExchangeRateUsed = rate,
                CostZar = costZar,
                Status = vm.Status
            };

            _context.ServiceRequests.Add(request);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task<List<SelectListItem>> LoadContracts()
        {
            return await _context.Contracts
                .Include(c => c.Client)
                .Select(c => new SelectListItem
                {
                    Value = c.ContractId.ToString(),
                    Text = $"{c.Client.Name} - Contract #{c.ContractId}"
                })
                .ToListAsync();
        }
    }
}