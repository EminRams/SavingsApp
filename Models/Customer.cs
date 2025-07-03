namespace SavingsApp.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Identification { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<SavingsAccount> SavingsAccounts { get; set; } = new List<SavingsAccount>();
}
