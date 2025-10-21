using BookstoreASPNetServer.Models;
using Microsoft.AspNetCore.Identity;

namespace BookstoreASPNetServer.Repositories
{
    public interface IAccountRepository
    {
        Task<IdentityResult> SignUp(SignupModel signupModel);
        Task<LoginResultModel?> Login(LoginModel loginModel);
    }
}
