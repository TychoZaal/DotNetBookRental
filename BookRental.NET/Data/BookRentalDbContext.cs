using BookRental.NET.Models;
using Microsoft.EntityFrameworkCore;

namespace BookRental.NET.Data
{
    public class BookRentalDbContext : DbContext
    {
        public BookRentalDbContext(DbContextOptions<BookRentalDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Admin WT",
                    Email = "WTAdmin@WT.com",
                    Password = "admin",
                    Location = "Oostzaan",
                    PhoneNumber = "1234",
                    StartingDate = DateTime.Now,
                    IsAdmin = true,
                    Token = ""
                },
                new User
                {
                    Id = 2,
                    Name = "Intern WT",
                    Email = "WTIntern@WT.com",
                    Password = "intern",
                    Location = "Groningen",
                    PhoneNumber = "5678",
                    StartingDate = DateTime.Now,
                    IsAdmin = false,
                    Token = ""
                },
                new User
                {
                    Id = 3,
                    Name = "Client User",
                    Email = "Client@ClientCompany.com",
                    Password = "client",
                    Location = "Amsterdam",
                    PhoneNumber = "password",
                    StartingDate = DateTime.Now,
                    IsAdmin = false,
                    Token = ""
                }
                );
        }
    }
}
