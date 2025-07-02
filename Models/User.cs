using System.ComponentModel.DataAnnotations;

namespace SavingsApp.Models;

public partial class User
{
    public int Id { get; set; }

    [Display(Name = "Nombre")]
    [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
    public string Username { get; set; } = null!;

    [Display(Name = "Correo Electrónico")]
    [Required(ErrorMessage = "El campo Correo Electrónico es obligatorio.")]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Display(Name = "Contraseña")]
    public string Password { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}
