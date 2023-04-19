using System.ComponentModel.DataAnnotations;

namespace BookRental.NET.Models.Dto
{
    public class BookDTOUpdate
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string ISBN { get; set; }
    }
}
