using BookstoreASPNetServer.Models;

namespace BookstoreASPNetServer.Repositories
{
    public interface ICartRepository
    {
        Task<List<ProductInCartModel>?> GetCartByUserId(string userId);
        Task<ProductInCartModel?> AddProductToCart(string userId, NewCartItemModel newCartItem);
        Task<List<ProductInCartModel>?> AddProductRangeToCart(string userId, List<NewCartItemModel> newCartItems);
        Task<ProductInCartModel?> RemoveProductFromCart(string userId, int productId);
        Task<List<ProductInCartModel>?> PlaceOrder(string userId);
        Task<ProductInCartModel?> UpdateProductQuantityInCart(string userId, NewCartItemModel productUpdate);
    }
}
