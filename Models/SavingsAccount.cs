using System.ComponentModel.DataAnnotations;

namespace SavingsApp.Models;

public partial class SavingsAccount
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    [Display(Name = "Número de Cuenta")]
    public string AccountNumber { get; set; } = null!;

    [Display(Name = "Balance")]
    public decimal Balance { get; set; }

    [Display(Name = "Descripción")]
    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    [Display(Name = "Cliente")]
    public virtual Customer? Customer { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
