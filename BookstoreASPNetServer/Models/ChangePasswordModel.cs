using System.ComponentModel.DataAnnotations;

namespace BookstoreASPNetServer.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Old password is required.")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "New password is required.")]
        public string NewPassword { get; set; }
    }
}
