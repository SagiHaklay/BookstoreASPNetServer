namespace BookstoreASPNetServer.Models
{
    public class CartItemModel
    {
        public int Id { get; set; }
        public BookModel Product { get; set; }
        public AppUser User { get; set; }
        public int Quantity { get; set; }
    }
}
