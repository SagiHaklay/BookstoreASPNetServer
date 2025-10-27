using BookstoreASPNetServer.Data;
using BookstoreASPNetServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookstoreASPNetServer.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly BookstoreContext _context;
        private readonly UserManager<AppUser> _userManager;
        public CartRepository(BookstoreContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<List<ProductInCartModel>?> AddProductRangeToCart(string userId, List<NewCartItemModel> newCartItems)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }
            List<CartItemModel> cartItems = new List<CartItemModel>();
            foreach (var newCartItem in newCartItems)
            {
                var book = await _context.Books.FindAsync(newCartItem.ProductId);
                if (book == null)
                {
                    return null;
                }
                var cartItem = new CartItemModel()
                {
                    User = user,
                    Product = book,
                    Quantity = newCartItem.Quantity
                };
                cartItems.Add(cartItem);
                book.CartItems ??= new List<CartItemModel>();
                book.CartItems.Add(cartItem);
            }
            if (user.CartItems != null)
            {
                user.CartItems = user.CartItems.Concat(cartItems).ToList();
            }
            else
            {
                user.CartItems = cartItems;
            }
            _context.AddRange(cartItems);
            await _context.SaveChangesAsync();
            
            return cartItems.Select(c => new ProductInCartModel() { 
                Product = new BookProductModel()
                {
                    Id = c.Product.Id.ToString(),
                    Name = c.Product.Name,
                    Publisher = c.Product.Publisher,
                    Price = c.Product.Price,
                    Discount = c.Product.Discount
                }, 
                Quantity = c.Quantity }).ToList();
        }

        public async Task<ProductInCartModel?> AddProductToCart(string userId, NewCartItemModel newCartItem)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }
            var book = await _context.Books.FindAsync(newCartItem.ProductId);
            if (book == null)
            {
                return null;
            }
            var cartItem = new CartItemModel()
            { 
                User = user,
                Product = book,
                Quantity = newCartItem.Quantity
            };
            user.CartItems ??= new List<CartItemModel>();
            user.CartItems.Add(cartItem);
            book.CartItems ??= new List<CartItemModel>();
            book.CartItems.Add(cartItem);
            _context.Add(cartItem);
            await _context.SaveChangesAsync();
            return new ProductInCartModel()
            {
                Product = new BookProductModel()
                {
                    Id = book.Id.ToString(),
                    Name = book.Name,
                    Publisher = book.Publisher,
                    Price = book.Price,
                    Discount = book.Discount
                },
                Quantity = newCartItem.Quantity
            };
        }

        public async Task<List<ProductInCartModel>?> GetCartByUserId(string userId)
        {
            var cartItems = await _context.Carts.Include(c => c.User).Where(c => c.User.Id == userId)
                .Include(c => c.Product).Select(c => new ProductInCartModel
                {
                    Product = new BookProductModel()
                    {
                        Id = c.Product.Id.ToString(),
                        Name = c.Product.Name,
                        Publisher = c.Product.Publisher,
                        Price = c.Product.Price,
                        Discount = c.Product.Discount
                    },
                    Quantity = c.Quantity
                }).ToListAsync();
            return cartItems;
        }

        public async Task<List<ProductInCartModel>?> PlaceOrder(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }
            var cartItems = await _context.Carts.Include(c => c.User).Where(c => c.User.Id == userId).Include(c => c.Product).ToListAsync();
            if (cartItems.Count == 0)
            {
                return null;
            }
            foreach (var cartItem in cartItems)
            {
                cartItem.Product.CartItems?.Remove(cartItem);
            }
            user.CartItems?.Clear();
            _context.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            return cartItems.Select(c => new ProductInCartModel() { 
                Product = new BookProductModel() { Id = c.Product.Id.ToString(), Name = c.Product.Name, Publisher = c.Product.Publisher, Price = c.Product.Price, Discount = c.Product.Discount }, 
                Quantity = c.Quantity }).ToList();
        }

        public async Task<ProductInCartModel?> RemoveProductFromCart(string userId, int productId)
        {
            var cartItem = await _context.Carts.Include(c => c.User).Where(c => c.User.Id == userId)
                .Include(c => c.Product).FirstOrDefaultAsync(c => c.Product.Id == productId);
            if (cartItem == null)
            {
                return null;
            }
            cartItem.User.CartItems?.Remove(cartItem);
            cartItem.Product.CartItems?.Remove(cartItem);
            _context.Remove(cartItem);
            await _context.SaveChangesAsync();
            return new ProductInCartModel()
            {
                Product = new BookProductModel()
                {
                    Id = cartItem.Product.Id.ToString(),
                    Name = cartItem.Product.Name,
                    Publisher = cartItem.Product.Publisher,
                    Price = cartItem.Product.Price,
                    Discount = cartItem.Product.Discount
                },
                Quantity = cartItem.Quantity
            };
        }
    }
}
