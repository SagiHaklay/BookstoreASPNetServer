namespace BookstoreASPNetServer.Models
{
    public class ProductInCartModel
    {
        public BookProductModel Product { get; set; }
        public int Quantity { get; set; }
    }
}
