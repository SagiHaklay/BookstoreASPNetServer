using System.ComponentModel.DataAnnotations;

namespace BookstoreASPNetServer.Models
{
    public class NewCartItemModel
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
