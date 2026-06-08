using GLMS.Api.Data;
using GLMS.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContractsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ContractsController(ApplicationDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    public async Task<IActionResult> GetContracts(
        DateTime? startDate,
        DateTime? endDate,
        string? status)
    {
        var query = _context.Contracts
            .Include(c => c.Client)
            .AsQueryable();

        if (startDate.HasValue)
            query = query.Where(c => c.StartDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(c => c.EndDate <= endDate.Value);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(c => c.Status.ToString() == status);

        var contracts = await query.ToListAsync();

        return Ok(contracts);
    }


    [HttpPost]
    public async Task<IActionResult> CreateContract([FromBody] Contract contract)
    {
        _context.Contracts.Add(contract);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetContractById),
            new { id = contract.ContractId },
            contract);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetContractById(int id)
    {
        var contract = await _context.Contracts
            .Include(c => c.Client)
            .FirstOrDefaultAsync(c => c.ContractId == id);

        if (contract == null)
            return NotFound();

        return Ok(contract);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        [FromBody] string status)
    {
        var contract = await _context.Contracts.FindAsync(id);

        if (contract == null)
            return NotFound();

        contract.Status =
            Enum.Parse<GLMS.Api.Enums.ContractStatus>(status);

        await _context.SaveChangesAsync();

        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContract(int id)
    {
        var contract = await _context.Contracts.FindAsync(id);

        if (contract == null)
            return NotFound();

        _context.Contracts.Remove(contract);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}