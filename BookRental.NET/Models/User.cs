using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookRental.NET.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Location { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime StartingDate { get; set; }
        public bool IsAdmin { get; set; }
        public string Token { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Loan> Loans { get; set; }

    }
}
