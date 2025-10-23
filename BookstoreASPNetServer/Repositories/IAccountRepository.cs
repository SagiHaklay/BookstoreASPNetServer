using BookstoreASPNetServer.Models;
using Microsoft.AspNetCore.Identity;

namespace BookstoreASPNetServer.Repositories
{
    public interface IAccountRepository
    {
        Task<IdentityResult> SignUp(SignupModel signupModel);
        Task<LoginResultModel?> Login(LoginModel loginModel);
        Task<UserDataModel?> GetUserById(string id);
        Task<UserDataModel?> UpdateUser(string id, UserDataModel updatedUser);
        Task<UserDataModel?> DeleteUser(string id);
        Task<string?> ChangePassword(string id, string newPassword, string oldPassword);
    }
}
