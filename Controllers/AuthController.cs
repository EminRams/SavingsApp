using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using SavingsApp.Models;
using System.Security.Claims;

namespace SavingsApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;

        private readonly SavingsAppContext _context;

        private readonly IToastNotification _toastNotification;

        public AuthController(SavingsAppContext context, IToastNotification toastNotification, ILogger<AuthController> logger)
        {
            _context = context;

            _toastNotification = toastNotification;

            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

            // Verificar que el usuario exite y la contraseña es correcta
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                // autenticar al usuario y crear el cookie de autenticación
                var claims = new List<Claim>
                {
                new (ClaimTypes.Name, user.Username),
                new (ClaimTypes.Email, user.Email),
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                _logger.LogInformation("{Username} ha iniciado sesión en el sistema", user.Username);

                return RedirectToAction("Index", "Home");
            }

            _logger.LogInformation("Intento de inico de sesión en el sistema");
            _toastNotification.AddWarningToastMessage("Credendiales Inválidas");

            return RedirectToAction("Login", "Auth");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Cerrar sesión y eliminar el cookie de autenticación
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            _logger.LogInformation("Un usuario se ha desconectado del sistema");

            return RedirectToAction("Index", "Home");
        }
    }
}
