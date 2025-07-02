namespace SavingsApp.Models;

public partial class SavingsAccount
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public string AccountNumber { get; set; } = null!;

    public decimal Balance { get; set; }

    public bool Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
