using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391.DTO;
using SWP391.Service;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WithdrawController : ControllerBase
    {
        private readonly IWithdrawService _withdrawService;
        private readonly IConfiguration _config;

        public WithdrawController(IConfiguration config, IWithdrawService withdrawService)
        {
            _config = config;
            _withdrawService = withdrawService;
        }

        [HttpGet("Get_All_Withdraws")]
        public async Task<IActionResult> GetAllWithdraws()
        {

            return await _withdrawService.HandleGetAllWithdraws();
        }

        [HttpGet("Get_Withdraw_By_UserId")]
        public async Task<IActionResult> GetWithdrawById([FromQuery] Guid id)
        {

            return await _withdrawService.HandleGetWithdrawByUserId(id);
        }

        [Authorize]
        [HttpPost("Create_Withdraw")]
        public async Task<IActionResult> CreateWithdraw([FromBody] WithdrawCreateDTO withdrawCreateDTO)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;

            return await _withdrawService.HandleCreateWithdraw(withdrawCreateDTO, userId);
        }

        [Authorize]
        [HttpPost("Update_Withdraw")]
        public async Task<IActionResult> UpdateWithdraw([FromBody] WithdrawUpdateDTO withdrawUpdateDTO)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;

            return await _withdrawService.HandleUpdateWithdraw(withdrawUpdateDTO, userId);
        }
    }
}
