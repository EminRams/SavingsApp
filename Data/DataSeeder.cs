using SavingsApp.Models;

namespace SavingsApp.Data
{
    public static class DataSeeder
    {
        public static void SeedUsers(SavingsAppContext context)
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

        public static void SeedCustomers(SavingsAppContext context)
        {
            if (context.Customers.Any()) return;

            var customers = new List<Customer>
            {
                new() { Name = "Jorge", Identification = "0101200400111", PhoneNumber="3343-7791", Email = "emin@example.com" },
                new() { Name = "Amy", Identification = "0101200400444", PhoneNumber="8134-2343", Email = "amy@example.com" },
                new() { Name = "Asensio", Identification = "0501200200999", PhoneNumber="9412-4575", Email = "asensio@example.com" },
                new() { Name = "Pedri", Identification = "0101200000345", PhoneNumber="3256-8623", Email = "pedri@example.com" },
                new() { Name = "Cristian", Identification = "0101197400561", PhoneNumber="8877-9313", Email = "cristian@example.com" },
            };

            context.Customers.AddRange(customers);
            context.SaveChanges();
        }
    }
}
