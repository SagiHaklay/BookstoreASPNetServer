using Microsoft.AspNetCore.Identity;

namespace BookstoreASPNetServer.Models
{
    public class AppUser : IdentityUser
    {
        public bool IsAdmin { get; set; }
        public IList<CartItemModel>? CartItems { get; set; }
    }
}
