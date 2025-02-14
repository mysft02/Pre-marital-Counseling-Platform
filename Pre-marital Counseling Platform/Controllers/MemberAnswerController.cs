using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391.Domain;
using SWP391.DTO.MemberAnswer;
using SWP391.DTO.Question;
using SWP391.Service;
using System.Security.Claims;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberAnswerController : ControllerBase
    {
        private readonly IMemberAnswerService _memberAnswerService;

        public MemberAnswerController(IMemberAnswerService memberAnswerService)
        {
            _memberAnswerService = memberAnswerService;
        }

        [HttpGet("Get_All_Member_Answer")]
        public async Task<IActionResult> GetAllAnswers()
        {
            var answers = await _memberAnswerService.GetAllMemberAnswers();
            return Ok(answers);
        }

        [HttpGet("Get_All_Member_Answer/{id}")]
        public async Task<IActionResult> GetAnswerById(Guid id)
        {
            var answer = await _memberAnswerService.GetMemberAnswerById(id);
            if (answer == null)
            {
                return NotFound();
            }
            return Ok(answer);
        }

        [Authorize]
        [HttpPost("Create_Member_Answer")]
        public async Task<IActionResult> CreateMemberAnswer([FromBody] CreateMemberAnswerDTO memberAnswerDTO)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await _memberAnswerService.CreateMemberAnswer(memberAnswerDTO, userId);
        }

        [Authorize]
        [HttpPost("Update_Member_Answer")]
        public async Task<IActionResult> UpdateMemberAnswer([FromBody] UpdateMemberAnswerDTO memberAnswerDTO)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await _memberAnswerService.UpdateMemberAnswer(memberAnswerDTO, userId);
        }
    }
}
