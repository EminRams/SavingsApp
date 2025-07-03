using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SavingsApp.Models;

namespace SavingsApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly SavingsAppContext _context;

        public AccountController(SavingsAppContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Recuperar la información del cliente y sus cuentas de ahorro
            var customer = await _context.Customers
                .Include(c => c.SavingsAccounts)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SavingsAccount account)
        {
            // Falta logica por implementar...
            await _context.SavingsAccounts.AddAsync(account);

            return RedirectToAction("Index", "Home");
        }
    }
}
