using System.ComponentModel.DataAnnotations;

namespace BookstoreASPNetServer.Models
{
    public class BookDataModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Author { get; set; }
        public string? Publisher { get; set; }
        [Required]
        public float Price { get; set; }
        [Range(0, 100)]
        public int? Discount { get; set; }
    }
}
