using System.ComponentModel.DataAnnotations;

namespace BookstoreASPNetServer.Models
{
    public class ChangePasswordModel
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
