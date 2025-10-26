using BookstoreASPNetServer.Data;
using BookstoreASPNetServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookstoreASPNetServer.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly BookstoreContext _bookstoreContext;

        public AccountRepository(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager, BookstoreContext bookstoreContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _bookstoreContext = bookstoreContext;
        }
        private async Task<string?> NewToken(AppUser user)
        {
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Count == 0)
            {
                await AddRoleToUser(user);
                roles = await _userManager.GetRolesAsync(user);
            }
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? ""));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<LoginResultModel?> Login(LoginModel loginModel)
        {
            var result = await _signInManager.PasswordSignInAsync(loginModel.Username, loginModel.Password, false, false);
            if (!result.Succeeded)
            {
                return null;
            }
            var user = await _userManager.FindByNameAsync(loginModel.Username);
            if (user == null)
            {
                return null;
            }
            var token = await NewToken(user);
            var loginResult = new LoginResultModel()
            {
                UserId = user.Id,
                IsAdmin = user.IsAdmin,
                Token = token
            };
            return loginResult;
        }

        public async Task<IdentityResult> SignUp(SignupModel signupModel)
        {
            AppUser user = new()
            {
                UserName = signupModel.Username,
                Email = signupModel.Email,
                IsAdmin = true
            };
            var result = await _userManager.CreateAsync(user, signupModel.Password);
            if (result.Succeeded)
            {
                await AddRoleToUser(user);
            }
            return result;
        }
        private async Task AddRoleToUser(AppUser user)
        {
            string roleName = user.IsAdmin ? "Admin" : "User";
            if (!(await _roleManager.RoleExistsAsync(roleName)))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<UserDataModel?> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            return new UserDataModel() 
            { 
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                IsAdmin = user.IsAdmin
            };
        }

        public async Task<UserDataModel?> UpdateUser(string id, UserDataModel updatedUser)
        {
            var user = await _userManager.FindByIdAsync(id);
            
            if (user == null)
            {
                return null;
            }
            IdentityResult? nameResult = null, emailResult = null;
            string? oldUsername = user.UserName;
            if (updatedUser.Username != null)
            {
                nameResult = await _userManager.SetUserNameAsync(user, updatedUser.Username);
                if (!nameResult.Succeeded)
                {
                    return null;
                }
            }
            if (updatedUser.Email != null)
            {
                emailResult = await _userManager.SetEmailAsync(user, updatedUser.Email);
                if (!emailResult.Succeeded)
                {
                    if (nameResult != null)
                    {
                        await _userManager.SetUserNameAsync(user, oldUsername);
                    }
                    return null;
                }
            }
            return new UserDataModel()
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                IsAdmin = user.IsAdmin
            };
        }

        public async Task<UserDataModel?> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            var userData = new UserDataModel()
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                IsAdmin = user.IsAdmin
            };
            var cartItems = await _bookstoreContext.Carts.Where(c => c.User == user).ToListAsync();
            if (cartItems != null)
            {
                _bookstoreContext.RemoveRange(cartItems);
                await _bookstoreContext.SaveChangesAsync();
            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return null;
            }
            return userData;
        }

        public async Task<string?> ChangePassword(string id, string newPassword, string oldPassword)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!result.Succeeded)
            {
                return null;
            }
            return newPassword;
        }
    }
}
