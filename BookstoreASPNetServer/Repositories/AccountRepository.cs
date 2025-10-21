using BookstoreASPNetServer.Models;
using Microsoft.AspNetCore.Identity;
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

        public AccountRepository(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
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
            var result = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, false, false);
            if (!result.Succeeded)
            {
                return null;
            }
            var user = await _userManager.FindByNameAsync(loginModel.UserName);
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
                UserName = signupModel.UserName,
                Email = signupModel.Email,
                IsAdmin = false
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
    }
}
