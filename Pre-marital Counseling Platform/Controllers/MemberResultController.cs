using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391.DTO.MemberResult;
using SWP391.Service;
using System.Security.Claims;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberResultController : ControllerBase
    {
        private readonly IMemberResultService _memberResultService;

        public MemberResultController(IMemberResultService memberResultService)
        {
            _memberResultService = memberResultService;
        }

        [HttpGet("Get_All_Member_Result")]
        public async Task<IActionResult> GetAllMemberResults()
        {
            return await _memberResultService.GetAllMemberResult();
        }

        [HttpGet("Get_Member_Result_By_Id")]
        public async Task<IActionResult> GetMemberResultById(Guid id)
        {
            return await _memberResultService.GetMemberResultById(id);
        }

        [Authorize]
        [HttpPost("Create_Member_Result")]
        public async Task<IActionResult> CreateMemberResult([FromBody] CreateMemberResultDTO memberResultDTO)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst(ClaimTypes.Sid)?.Value;
            return await _memberResultService.CreateMemberResult(memberResultDTO, userId);
        }

        [Authorize]
        [HttpPost("Update_Member_Result")]
        public async Task<IActionResult> UpdateMemberResult([FromBody] UpdateMemberResultDTO memberResultDTO)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst(ClaimTypes.Sid)?.Value;
            return await _memberResultService.UpdateMemberResult(memberResultDTO, userId);
        }
    }
}
