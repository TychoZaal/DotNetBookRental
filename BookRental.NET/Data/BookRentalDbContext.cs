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
        public DbSet<Book> Books { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

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

            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Title = "Knight of the Seven Kingdoms",
                    Author = "George R. R. Martin",
                    ISBN = "1234-5678-4200"
                },
                new Book
                {
                    Id = 2,
                    Title = "Shadow and Bone",
                    Author = "Leigh Bardugo",
                    ISBN = "8972-2387-2873"
                },
                new Book
                {
                    Id = 3,
                    Title = "Lord of the Rings",
                    Author = "J. R. R. Tolkien",
                    ISBN = "2890-5498-1283"
                }
                );
        }
    }
}
