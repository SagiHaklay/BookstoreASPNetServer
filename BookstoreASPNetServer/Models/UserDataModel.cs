using System.ComponentModel.DataAnnotations;

namespace BookstoreASPNetServer.Models
{
    public class UserDataModel
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public bool? IsAdmin { get; set; }
    }
}
