using BookstoreASPNetServer.Repositories;
using BookstoreASPNetServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreASPNetServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart([FromRoute] string userId)
        {
            var cart = await _cartRepository.GetCartByUserId(userId);
            cart ??= new List<ProductInCartModel>();
            return Ok(cart);
        }
        [HttpPost("{userId}/add")]
        public async Task<IActionResult> AddToCart([FromRoute] string userId, [FromBody] NewCartItemModel newCartItem)
        {
            var result = await _cartRepository.AddProductToCart(userId, newCartItem);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpPost("{userId}/addMany")]
        public async Task<IActionResult> AddManyToCart([FromRoute] string userId, [FromBody] List<NewCartItemModel> cartItems)
        {
            if (cartItems.Count == 0)
            {
                return BadRequest();
            }
            var result = await _cartRepository.AddProductRangeToCart(userId, cartItems);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpDelete("{userId}/delete/{productId}")]
        public async Task<IActionResult> RemoveProduct([FromRoute] string userId, [FromRoute] int productId)
        {
            var result = await _cartRepository.RemoveProductFromCart(userId, productId);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpPost("{userId}/order")]
        public async Task<IActionResult> PlaceOrder([FromRoute] string userId)
        {
            var result = await _cartRepository.PlaceOrder(userId);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}
