using System.ComponentModel.DataAnnotations;

namespace BookRental.NET.Models.Dto
{
    public class LoanDTOCreate
    {
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public Book Book { get; set; }
    }
}
