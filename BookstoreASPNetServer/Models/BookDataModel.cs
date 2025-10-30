using System.ComponentModel.DataAnnotations;

namespace BookstoreASPNetServer.Models
{
    public class BookDataModel
    {
        [Required(ErrorMessage = "Book name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Author is required.")]
        public string Author { get; set; }
        public string? Publisher { get; set; }
        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "Price cannot be negative.")]
        public float Price { get; set; }
        [Range(0, 100, ErrorMessage = "Percentage must be between 0 and 100.")]
        public int? Discount { get; set; }
        public string? ImageUrl { get; set; }
    }
}
