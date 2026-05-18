using GLMS.Api.Data;
using GLMS.Api.Models;
using GLMS.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceRequestsController : ControllerBase
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

    // GET: api/servicerequests
    [HttpGet]
    public async Task<IActionResult> GetRequests()
    {
        var requests = await _context.ServiceRequests
            .Include(s => s.Contract)
            .ToListAsync();

        return Ok(requests);
    }

    // GET: api/servicerequests/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRequest(int id)
    {
        var request = await _context.ServiceRequests
            .Include(s => s.Contract)
            .FirstOrDefaultAsync(s => s.ServiceRequestId == id);

        if (request == null)
            return NotFound();

        return Ok(request);
    }

    // POST: api/servicerequests
    [HttpPost]
    public async Task<IActionResult> CreateRequest(
        [FromBody] ServiceRequest request)
    {
        if (request.CostUsd <= 0)
            return BadRequest("USD amount must be greater than zero.");

        var contract = await _context.Contracts.FindAsync(request.ContractId);

        if (contract == null)
            return BadRequest("Contract not found.");

        if (!_contractRulesService.CanCreateServiceRequest(contract))
        {
            return BadRequest(
                "Cannot create service request for Expired or On Hold contracts.");
        }

        var rate = await _currencyService.GetUsdToZarRateAsync();

        request.ExchangeRateUsed = rate;

        request.CostZar = _currencyService.ConvertUsdToZar(
            request.CostUsd,
            rate);

        _context.ServiceRequests.Add(request);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetRequest),
            new { id = request.ServiceRequestId },
            request);
    }

    // DELETE: api/servicerequests/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRequest(int id)
    {
        var request = await _context.ServiceRequests.FindAsync(id);

        if (request == null)
            return NotFound();

        _context.ServiceRequests.Remove(request);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}