using GLMS.Web.Models;
using GLMS.Web.Services;
using GLMS.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLMS.Web.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly IServiceRequestsApiService _serviceRequestApi;
        private readonly IContractsApiService _contractsApi;

        public ServiceRequestsController(
            IServiceRequestsApiService serviceRequestApi,
            IContractsApiService contractsApi)
        {
            _serviceRequestApi = serviceRequestApi;
            _contractsApi = contractsApi;
        }

        // GET: ServiceRequests
        public async Task<IActionResult> Index()
        {
            var requests =
                await _serviceRequestApi.GetServiceRequestsAsync();

            return View(requests);
        }

        // GET: ServiceRequests/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var request =
                await _serviceRequestApi
                    .GetServiceRequestByIdAsync(id);

            if (request == null)
                return NotFound();

            return View(request);
        }

        // GET: ServiceRequests/Create
        public async Task<IActionResult> Create()
        {
            var contracts =
                await _contractsApi.GetContractsAsync();

            var vm = new ServiceRequestCreateViewModel
            {
                Contracts = contracts.Select(c =>
                    new SelectListItem
                    {
                        Value = c.ContractId.ToString(),
                        Text =
                            $"Contract #{c.ContractId}"
                    }).ToList()
            };

            return View(vm);
        }

        // POST: ServiceRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            ServiceRequestCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var contracts =
                    await _contractsApi.GetContractsAsync();

                vm.Contracts = contracts.Select(c =>
                    new SelectListItem
                    {
                        Value = c.ContractId.ToString(),
                        Text =
                            $"Contract #{c.ContractId}"
                    }).ToList();

                return View(vm);
            }

            var request = new ServiceRequest
            {
                ContractId = vm.ContractId,
                Description = vm.Description,
                CostUsd = vm.CostUsd,
                Status = vm.Status
            };

            await _serviceRequestApi
                .CreateServiceRequestAsync(request);

            return RedirectToAction(nameof(Index));
        }

        // GET: ServiceRequests/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var request =
                await _serviceRequestApi
                    .GetServiceRequestByIdAsync(id);

            if (request == null)
                return NotFound();

            return View(request);
        }

        // POST: ServiceRequests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            ServiceRequest request)
        {
            if (id != request.ServiceRequestId)
                return NotFound();

            if (!ModelState.IsValid)
                return View(request);

            await _serviceRequestApi
                .UpdateServiceRequestAsync(id, request);

            return RedirectToAction(nameof(Index));
        }

        // GET: ServiceRequests/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var request =
                await _serviceRequestApi
                    .GetServiceRequestByIdAsync(id);

            if (request == null)
                return NotFound();

            return View(request);
        }

        // POST: ServiceRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _serviceRequestApi
                .DeleteServiceRequestAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}