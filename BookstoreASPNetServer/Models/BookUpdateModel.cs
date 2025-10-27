using System.ComponentModel.DataAnnotations;

namespace BookstoreASPNetServer.Models
{
    public class BookUpdateModel
    {
        public string? Name { get; set; }
        public string? Author { get; set; }
        public string? Publisher { get; set; }
        public float? Price { get; set; }
        [Range(0, 100)]
        public int? Discount { get; set; }
        public string? ImageUrl { get; set; }
    }
}
