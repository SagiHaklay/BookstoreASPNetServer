namespace BookstoreASPNetServer.Models
{
    public class LoginResultModel
    {
        public string UserId { get; set; }
        public bool IsAdmin { get; set; }
        public string? Token { get; set; }
    }
}
