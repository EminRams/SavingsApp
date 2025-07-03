namespace SavingsApp.Models.ViewModels;

using System.ComponentModel.DataAnnotations;
using SavingsApp.Models;

public class TransactionViewModel
{
    public int Id { get; set; }

    [Display(Name = "Tipo de Transacción")]
    public string Type { get; set; } = null!;

    [Display(Name = "Monto")]
    public decimal Amount { get; set; }

    [Display(Name = "Fecha de Transacción")]
    public DateTime TransactionDate { get; set; }

    [Display(Name = "Fecha de Creación")]
    public DateTime? CreatedAt { get; set; }

    [Display(Name = "Cuenta de Ahorro")]
    public virtual SavingsAccount SavingsAccount { get; set; } = null!;

    [Display(Name = "Saldo Resultante")]
    public decimal BalanceAfterTransaction { get; set; }
}
