using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SavingsApp.Models;

namespace SavingsApp.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly SavingsAppContext _context;

        public TransactionController(SavingsAppContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageSize = 10, int page = 1)
        {
            var transactions = await _context.Transactions
                .OrderBy(t => t.TransactionDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(t => t.SavingsAccount)
                .ThenInclude(a => a.Customer)
                .ToListAsync();

            var totalTransactions = await _context.Transactions.CountAsync();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling(totalTransactions / (double)pageSize);

            return View(transactions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int AccountId, string type, decimal amount)
        {
            var account = await _context.SavingsAccounts.FindAsync(AccountId);

            if (account == null)
                return NotFound();

            // Validar monto negativo
            if (amount <= 0)
            {
                return RedirectToAction("Details", "Account", new { id = account.CustomerId });
            }

            if (type == "withdrawal")
            {
                // Validar que no supere $5000 diarios
                var today = DateTime.Today;

                var todayWithdrawals = await _context.Transactions
                    .Where(t => t.SavingsAccountId == AccountId &&
                                t.Type == "withdrawal" &&
                                t.TransactionDate.Date == today)
                    .SumAsync(t => t.Amount);

                if (todayWithdrawals + amount > 5000)
                {
                    var restante = 5000 - todayWithdrawals; // mostrarlo en una alerta
                    return RedirectToAction("Details", "Account", new { id = account.CustomerId });
                }

                // Validar que no supere el saldo de la cuenta
                if (amount > account.Balance)
                {
                    return RedirectToAction("Details", "Account", new { id = account.CustomerId });
                }

                // Actualizar el saldo de la cuenta
                account.Balance -= amount;
            }
            else
            {
                // Actualizar el saldo de la cuenta
                account.Balance += amount;
            }

            var transaction = new Transaction
            {
                SavingsAccountId = AccountId,
                Amount = amount,
                Type = type
            };


            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Account", new { id = account.CustomerId });
        }

    }
}
