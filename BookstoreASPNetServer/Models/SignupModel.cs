using System.ComponentModel.DataAnnotations;

namespace BookstoreASPNetServer.Models
{
    public class SignupModel
    {
        [Required(ErrorMessage = "User name is required.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        
    }
}
