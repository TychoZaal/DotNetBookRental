using System.ComponentModel.DataAnnotations;

namespace BookRental.NET.Models.Dto
{
    public class UserDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        [Required]
        [MaxLength(50)]
        public string Location { get; set; }
        [Required]
        [MaxLength(12)]
        public string PhoneNumber { get; set; }
        public DateTime StartingDate { get; set; }
    }
}
