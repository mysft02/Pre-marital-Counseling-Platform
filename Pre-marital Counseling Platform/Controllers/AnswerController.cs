using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391.DTO.Answer;
using SWP391.DTO.MemberAnswer;
using SWP391.Service;
using System.Security.Claims;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _answerService;

        public AnswerController(IAnswerService answerService)
        {
            _answerService = answerService;
        }

        [HttpGet("Get_All_Answer")]
        public async Task<IActionResult> GetAllAnswer()
        {
            return await _answerService.GetAllAnswer();
        }

        [HttpGet("Get_Answe_By_Id")]
        public async Task<IActionResult> GetAnswerById([FromQuery] Guid id)
        {
            return await _answerService.GetAnswerById(id);
        }

        [Authorize]
        [HttpPost("Create_Answer")]
        public async Task<IActionResult> CreateAnswer([FromBody] CreateAnswerDTO dto)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;
            return await _answerService.CreateAnswer(dto, userId);
        }

        [Authorize]
        [HttpPost("Update_Answer")]
        public async Task<IActionResult> UpdateAnswer([FromBody] UpdateAnswerDTO dto)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;
            return await _answerService.UpdateAnswer(dto, userId);
        }
    }
}
