namespace SavingsApp.Models.ViewModels;

using SavingsApp.Models;

public class TransactionViewModel
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime TransactionDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual SavingsAccount SavingsAccount { get; set; } = null!;

    public decimal BalanceAfterTransaction { get; set; }
}
