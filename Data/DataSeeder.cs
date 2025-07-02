using SavingsApp.Models;
using BCrypt.Net;

namespace SavingsApp.Data
{
    public static class DataSeeder
    {
        public static void Initialize(SavingsAppContext context)
        {
            if (context.Users.Any()) return;

            var users = new List<User>
            {
                new() { Username = "emin", Email = "emin@example.com", Password = BCrypt.Net.BCrypt.HashPassword("secret123") },
                new() { Username = "john", Email = "john@example.com", Password = BCrypt.Net.BCrypt.HashPassword("john1234") },
                new() { Username = "jose", Email = "jose@example.com", Password = BCrypt.Net.BCrypt.HashPassword("jose123") },
                new() { Username = "jessie", Email = "jessie@example.com", Password = BCrypt.Net.BCrypt.HashPassword("jessie12") },
                new() { Username = "luis", Email = "luis@example.com", Password = BCrypt.Net.BCrypt.HashPassword("luis789") }
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
