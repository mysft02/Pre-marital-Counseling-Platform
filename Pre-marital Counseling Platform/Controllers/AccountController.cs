using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391.DTO;
using SWP391.Service;
using System.Security.Claims;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountController;

        public AccountController(IAccountService accountController)
        {
            _accountController = accountController;
        }

        [Authorize]
        [HttpPost("Change_Password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePassword)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst(ClaimTypes.Sid)?.Value;

            return await _accountController.ChangePassword(changePassword, userId);
        }

        [AllowAnonymous]
        [HttpGet("Get_All_Users")]
        public async Task<IActionResult> GetAllUsers()
        {

            return await _accountController.GetAllUsers();
        }
    }
}
