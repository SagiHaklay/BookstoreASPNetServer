using BookstoreASPNetServer.Models;
using BookstoreASPNetServer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreASPNetServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public UserController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] string id)
        {
            var user = await _accountRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPatch("{id}/update")]
        public async Task<IActionResult> UpdateUser([FromRoute] string id, [FromBody] UserDataModel updatedUser)
        {
            var user = await _accountRepository.UpdateUser(id, updatedUser);
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }
        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            var user = await _accountRepository.DeleteUser(id);
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }
        [HttpPatch("{id}/password")]
        public async Task<IActionResult> ChangePassword([FromRoute] string id, [FromBody] ChangePasswordModel changePassword)
        {
            var result = await _accountRepository.ChangePassword(id, changePassword.NewPassword, changePassword.OldPassword);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}
