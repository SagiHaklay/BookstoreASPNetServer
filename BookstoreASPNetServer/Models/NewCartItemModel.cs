using System.ComponentModel.DataAnnotations;

namespace BookstoreASPNetServer.Models
{
    public class NewCartItemModel
    {
        [Required(ErrorMessage = "Product is required.")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be positive.")]
        public int Quantity { get; set; }
    }
}
