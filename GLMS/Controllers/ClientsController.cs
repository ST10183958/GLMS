using GLMS.Web.Models;
using GLMS.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace GLMS.Web.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IClientsApiService _apiService;

        public ClientsController(IClientsApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            var clients = await _apiService.GetClientsAsync();

            return View(clients);
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var client = await _apiService.GetClientByIdAsync(id);

            if (client == null)
                return NotFound();

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            if (!ModelState.IsValid)
                return View(client);

            await _apiService.CreateClientAsync(client);

            return RedirectToAction(nameof(Index));
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var client = await _apiService.GetClientByIdAsync(id);

            if (client == null)
                return NotFound();

            return View(client);
        }

        // POST: Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            if (id != client.ClientId)
                return NotFound();

            if (!ModelState.IsValid)
                return View(client);

            await _apiService.UpdateClientAsync(id, client);

            return RedirectToAction(nameof(Index));
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _apiService.GetClientByIdAsync(id);

            if (client == null)
                return NotFound();

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _apiService.DeleteClientAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}