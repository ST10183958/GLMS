using System.Text;
using System.Text.Json;
using GLMS.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GLMS.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // =========================
        // LOGIN
        // =========================

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                var client = _httpClientFactory.CreateClient("GLMSApi");

                var payload = new
                {
                    username = vm.Username,
                    password = vm.Password
                };

                var json = JsonSerializer.Serialize(payload);

                var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await client.PostAsync(
                    "api/auth/login",
                    content
                );

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(
                        "",
                        "Invalid username or password."
                    );

                    return View(vm);
                }

                var responseContent =
                    await response.Content.ReadAsStringAsync();

                using var doc =
                    JsonDocument.Parse(responseContent);

                var token = doc.RootElement
                    .GetProperty("token")
                    .GetString();

                if (string.IsNullOrWhiteSpace(token))
                {
                    ModelState.AddModelError(
                        "",
                        "Authentication token was not returned."
                    );

                    return View(vm);
                }

                // SAVE JWT TOKEN TO SESSION
                HttpContext.Session.SetString(
                    "JWToken",
                    token
                );

                HttpContext.Session.SetString(
                    "Username",
                    vm.Username
                );

                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    $"Authentication server error: {ex.Message}"
                );

                return View(vm);
            }
        }

        // =========================
        // REGISTER
        // =========================

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                var client = _httpClientFactory.CreateClient("GLMSApi");

                var payload = new
                {
                    username = vm.Username,
                    email = vm.Email,
                    password = vm.Password
                };

                var json = JsonSerializer.Serialize(payload);

                var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await client.PostAsync(
                    "api/auth/register",
                    content
                );

                if (!response.IsSuccessStatusCode)
                {
                    var error =
                        await response.Content.ReadAsStringAsync();

                    ModelState.AddModelError(
                        "",
                        $"Registration failed: {error}"
                    );

                    return View(vm);
                }

                TempData["SuccessMessage"] =
                    "Registration successful. Please login.";

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    $"Registration server error: {ex.Message}"
                );

                return View(vm);
            }
        }

        // =========================
        // LOGOUT
        // =========================

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(
                "Login",
                "Auth"
            );
        }

        // =========================
        // ACCESS DENIED
        // =========================

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}