using System.ComponentModel.DataAnnotations;

namespace BookRental.NET.Models.Dto
{
    public class ReservationDTOCreate
    {
        public string Status { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public Book Book { get; set; }
    }
}
