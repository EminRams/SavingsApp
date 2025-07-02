namespace SavingsApp.Models;

public partial class AuditLog
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public string Action { get; set; } = null!;

    public string? Details { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Customer? Customer { get; set; }
}
