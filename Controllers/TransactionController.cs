using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using SavingsApp.Models;
using SavingsApp.Models.ViewModels;

namespace SavingsApp.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly SavingsAppContext _context;

        private readonly IToastNotification _toastNotification;

        public TransactionController(SavingsAppContext context, IToastNotification toastNotification)
        {
            _context = context;

            _toastNotification = toastNotification;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageSize = 10, int page = 1)
        {
            var allTransactions = await _context.Transactions
                .Include(t => t.SavingsAccount)
                .ThenInclude(a => a.Customer)
                .OrderBy(t => t.TransactionDate)
                .ToListAsync();

            // Calcular balance acumulados por cuenta
            var balance = new Dictionary<int, decimal>();
            var transactionVMs = new List<TransactionViewModel>();

            foreach (var t in allTransactions)
            {
                var accountId = t.SavingsAccount.Id;

                if (!balance.ContainsKey(accountId))
                    balance[accountId] = 0;

                if (t.Type == "deposit")
                    balance[accountId] += t.Amount;
                else if (t.Type == "withdrawal")
                    balance[accountId] -= t.Amount;

                transactionVMs.Add(new TransactionViewModel
                {
                    Id = t.Id,
                    Type = t.Type,
                    Amount = t.Amount,
                    TransactionDate = t.TransactionDate,
                    CreatedAt = t.CreatedAt,
                    SavingsAccount = t.SavingsAccount,
                    BalanceAfterTransaction = balance[accountId]
                });
            }

            // Aplicar paginación
            int total = transactionVMs.Count;
            int totalPages = (int)Math.Ceiling(total / (double)pageSize);

            var paginated = transactionVMs
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            return View(paginated);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int AccountId, string type, decimal amount, DateTime? transactionDate)
        {
            var account = await _context.SavingsAccounts.FindAsync(AccountId);

            transactionDate ??= DateTime.Now;

            if (account == null)
                return NotFound();

            // Validar monto negativo
            if (amount <= 0)
            {
                _toastNotification.AddErrorToastMessage("Monto Inválido");
                return RedirectToAction("Details", "Account", new { id = account.CustomerId });
            }

            if (type == "withdrawal")
            {
                // Validar que no supere $5000 diarios
                var todayWithdrawals = await _context.Transactions
                    .Where(t => t.SavingsAccountId == AccountId &&
                                t.Type == "withdrawal" &&
                                t.TransactionDate.Date == transactionDate)
                    .SumAsync(t => t.Amount);

                if (todayWithdrawals + amount > 5000)
                {
                    _toastNotification.AddErrorToastMessage("No puede retirar más de $5000 diarios.");
                    return RedirectToAction("Details", "Account", new { id = account.CustomerId });
                }

                // Validar que no supere el saldo de la cuenta
                if (amount > account.Balance)
                {
                    _toastNotification.AddErrorToastMessage("Saldo insuficiente para realizar el retiro.");
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
                Type = type,
                TransactionDate = transactionDate.Value
            };


            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            _toastNotification.AddSuccessToastMessage("Transacción realizada correctamente.");

            return RedirectToAction("Details", "Account", new { id = account.CustomerId });
        }

    }
}
