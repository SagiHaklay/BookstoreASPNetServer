using System.ComponentModel.DataAnnotations;

namespace BookstoreASPNetServer.Models
{
    public class SignupModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        
    }
}
