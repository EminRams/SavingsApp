using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NToastNotify;
using NToastNotify.Helpers;
using SavingsApp.Models;

namespace SavingsApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        private readonly SavingsAppContext _context;

        private readonly IToastNotification _toastNotification;

        public AccountController(SavingsAppContext context, IToastNotification toastNotification, ILogger<AccountController> logger)
        {
            _context = context;
            _toastNotification = toastNotification;
            _logger = logger;
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? customerId, string? description)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var account = new SavingsAccount
                {
                    CustomerId = customer.Id,
                    AccountNumber = GenerateAccountNumber(_context),
                    Description = description,
                    Balance = 0
                };

                _context.SavingsAccounts.Add(account);
                await _context.SaveChangesAsync();

                _toastNotification.AddSuccessToastMessage("Cuenta de ahorro creada correctamente.");
                _logger.LogInformation("Creacion de cuenta de ahorro {account}", account.ToJson());
                return RedirectToAction("Details", new { id = customer.Id });
            }

            _toastNotification.AddSuccessToastMessage("Error al crear cuenta de ahorro");
            _logger.LogError("Error al crear cuenta de ahorro");
            return RedirectToAction("Details", new { id = customer.Id });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int customerId, string? description, string accountNumber, decimal balance)
        {
            var account = await _context.SavingsAccounts.FindAsync(id);

            if (account == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                account.Description = description;
                account.AccountNumber = accountNumber;
                account.Balance = balance;

                await _context.SaveChangesAsync();

                _toastNotification.AddSuccessToastMessage("Cuenta de ahorro actualizada correctamente.");
                _logger.LogInformation("Actualizacion de cuenta de ahorro {account}", account.ToJson());
                return RedirectToAction("Details", new { id = customerId });
            }

            _toastNotification.AddErrorToastMessage("Error al actualizar la cuenta de ahorro.");
            _logger.LogError("Error al actualizar cuenta de ahorro {account}", account);
            return RedirectToAction("Details", new { id = customerId });
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
