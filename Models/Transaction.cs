namespace SavingsApp.Models;

public partial class Transaction
{
    public int Id { get; set; }

    public int SavingsAccountId { get; set; }

    public string Type { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime TransactionDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual SavingsAccount SavingsAccount { get; set; } = null!;
}
