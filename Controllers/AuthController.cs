using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SavingsApp.Models;
using System.Security.Claims;

namespace SavingsApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly SavingsAppContext _context;

        public AuthController(SavingsAppContext context)
        {
            _context = context;
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

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Credenciales Inválidas");
            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Cerrar sesión y eliminar el cookie de autenticación
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}
