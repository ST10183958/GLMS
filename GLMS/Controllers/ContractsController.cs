using GLMS.Web.Data;
using GLMS.Web.Models;
using GLMS.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Web.Controllers
{
    public class ContractsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public ContractsController(ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, string? status)
        {
            var query = _context.Contracts.Include(c => c.Client).AsQueryable();

            if (startDate.HasValue)
                query = query.Where(c => c.StartDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(c => c.EndDate <= endDate.Value);

            if (!string.IsNullOrWhiteSpace(status) &&
                Enum.TryParse<GLMS.Web.Enums.ContractStatus>(status, out var parsedStatus))
            {
                query = query.Where(c => c.Status == parsedStatus);
            }

            var contracts = await query.ToListAsync();
            return View(contracts);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.ClientId = new SelectList(await _context.Clients.ToListAsync(), "ClientId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contract contract, IFormFile signedAgreement)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ClientId = new SelectList(await _context.Clients.ToListAsync(), "ClientId", "Name", contract.ClientId);
                return View(contract);
            }

            if (signedAgreement != null)
            {
                try
                {
                    contract.SignedAgreementFilePath = await _fileService.SaveContractPdfAsync(signedAgreement);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("SignedAgreementFilePath", ex.Message);
                    ViewBag.ClientId = new SelectList(await _context.Clients.ToListAsync(), "ClientId", "Name", contract.ClientId);
                    return View(contract);
                }
            }

            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DownloadAgreement(int ContractId)
        {
            var contract = await _context.Contracts.FindAsync(ContractId);
            if (contract == null || string.IsNullOrWhiteSpace(contract.SignedAgreementFilePath))
                return NotFound();

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", contract.SignedAgreementFilePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            var fileBytes = await System.IO.File.ReadAllBytesAsync(fullPath);

            return File(fileBytes, "application/pdf", Path.GetFileName(fullPath));
        }
    }
}