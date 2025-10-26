namespace BookstoreASPNetServer.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string? Publisher { get; set; }
        public float Price { get; set; }
        public int? Discount { get; set; }
        public string? ImageUrl { get; set; }
        public IList<CartItemModel>? CartItems { get; set; }
    }
}
