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

        [HttpPost]
        public async Task<IActionResult> Create(int? customerId, string? description)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null)
            {
                return NotFound();
            }

            var account = new SavingsAccount
            {
                CustomerId = customer.Id,
                AccountNumber = GenerateAccountNumber(_context),
                Description = description,
                Balance = 0
            };

            _context.SavingsAccounts.Add(account);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = customer.Id });
        }

        public static string GenerateAccountNumber(SavingsAppContext context)
        {
            // Generar un número de cuenta aleatorio
            var random = new Random();
            string accountNumber;
            do
            {
                // Verificar que no haya duplicados
                accountNumber = random.Next(100000000, 999999999).ToString();
            } while (context.SavingsAccounts.Any(a => a.AccountNumber == accountNumber));

            return accountNumber;
        }
    }
}
