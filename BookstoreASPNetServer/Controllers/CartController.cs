using BookstoreASPNetServer.Repositories;
using BookstoreASPNetServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BookstoreASPNetServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IAccountRepository _accountRepository;
        public CartController(ICartRepository cartRepository, IAccountRepository accountRepository)
        {
            _cartRepository = cartRepository;
            _accountRepository = accountRepository;
        }

        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetCart([FromRoute] string userId)
        {
            bool isValid = await _accountRepository.ValidateUserId(userId, User.Identity?.Name);
            if (!isValid)
            {
                return Unauthorized();
            }
            var cart = await _cartRepository.GetCartByUserId(userId);
            cart ??= new List<ProductInCartModel>();
            return Ok(cart);
        }
        [HttpPost("{userId}/add")]
        [Authorize]
        public async Task<IActionResult> AddToCart([FromRoute] string userId, [FromBody] NewCartItemModel newCartItem)
        {
            bool isValid = await _accountRepository.ValidateUserId(userId, User.Identity?.Name);
            if (!isValid)
            {
                return Unauthorized();
            }
            var result = await _cartRepository.AddProductToCart(userId, newCartItem);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpPost("{userId}/addMany")]
        [Authorize]
        public async Task<IActionResult> AddManyToCart([FromRoute] string userId, [FromBody] CartItemList cartItemList)
        {
            bool isValid = await _accountRepository.ValidateUserId(userId, User.Identity?.Name);
            if (!isValid)
            {
                return Unauthorized();
            }
            if (cartItemList.CartItems.Count == 0)
            {
                return BadRequest();
            }
            var result = await _cartRepository.AddProductRangeToCart(userId, cartItemList.CartItems);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpDelete("{userId}/delete/{productId}")]
        [Authorize]
        public async Task<IActionResult> RemoveProduct([FromRoute] string userId, [FromRoute] int productId)
        {
            bool isValid = await _accountRepository.ValidateUserId(userId, User.Identity?.Name);
            if (!isValid)
            {
                return Unauthorized();
            }
            var result = await _cartRepository.RemoveProductFromCart(userId, productId);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpPost("{userId}/order")]
        [Authorize]
        public async Task<IActionResult> PlaceOrder([FromRoute] string userId)
        {
            bool isValid = await _accountRepository.ValidateUserId(userId, User.Identity?.Name);
            if (!isValid)
            {
                return Unauthorized();
            }
            var result = await _cartRepository.PlaceOrder(userId);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}
