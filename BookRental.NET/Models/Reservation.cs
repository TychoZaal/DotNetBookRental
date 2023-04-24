using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookRental.NET.Models
{
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public Book Book { get; set; }
    }
}
