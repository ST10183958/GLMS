using GLMS.Api.Data;
using GLMS.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ClientsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/clients
    [HttpGet]
    public async Task<IActionResult> GetClients()
    {
        var clients = await _context.Clients
            .Include(c => c.Contracts)
            .ToListAsync();

        return Ok(clients);
    }

    // GET: api/clients/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetClient(int id)
    {
        var client = await _context.Clients
            .Include(c => c.Contracts)
            .FirstOrDefaultAsync(c => c.ClientId == id);

        if (client == null)
            return NotFound();

        return Ok(client);
    }

    // POST: api/clients
    [HttpPost]
    public async Task<IActionResult> CreateClient([FromBody] Client client)
    {
        _context.Clients.Add(client);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetClient),
            new { id = client.ClientId },
            client);
    }

    // PUT: api/clients/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClient(int id, [FromBody] Client client)
    {
        if (id != client.ClientId)
            return BadRequest();

        var existing = await _context.Clients.FindAsync(id);

        if (existing == null)
            return NotFound();

        existing.Name = client.Name;
        existing.ContactDetails = client.ContactDetails;
        existing.Region = client.Region;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/clients/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        var client = await _context.Clients
            .Include(c => c.Contracts)
            .FirstOrDefaultAsync(c => c.ClientId == id);

        if (client == null)
            return NotFound();

        _context.Clients.Remove(client);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}