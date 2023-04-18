using System.ComponentModel.DataAnnotations;

namespace BookRental.NET.Models.Dto
{
    public class UserDTOUpdate
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
        public string Location { get; set; }
        [Required]
        [MaxLength(12)]
        public string PhoneNumber { get; set; }
        public DateTime StartingDate { get; set; }
        public bool IsAdmin { get; set; }
    }
}
