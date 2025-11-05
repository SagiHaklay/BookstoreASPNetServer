using System.ComponentModel.DataAnnotations;

namespace BookstoreASPNetServer.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Old password is required.")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "New password is required.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$",
        ErrorMessage = "Password must be at least 6 characters and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string NewPassword { get; set; }
    }
}
