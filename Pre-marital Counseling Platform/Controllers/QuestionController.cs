using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391.DTO.Question;
using SWP391.DTO.Quiz;
using SWP391.Service;
using System.Security.Claims;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IConfiguration _config;

        public QuestionController(IConfiguration config, IQuestionService questionService)
        {
            _config = config;
            _questionService = questionService;
        }

        [HttpGet("Get_All_Question")]
        public async Task<IActionResult> GetAllQuestions()
        {

            return await _questionService.HandleGetAllQuestions();
        }

        [HttpGet("Get_Question_By_Id")]
        public async Task<IActionResult> GetQuestionById([FromQuery] Guid id)
        {

            return await _questionService.HandleGetQuestionById(id);
        }

        [Authorize]
        [HttpPost("Create_Question")]
        public async Task<IActionResult> CreateQuestion([FromBody] QuestionCreateDTO questionCreateDTO)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;

            return await _questionService.HandleCreateQuestion(questionCreateDTO, userId);
        }

        [Authorize]
        [HttpPost("Update_Question")]
        public async Task<IActionResult> UpdateQuestion([FromBody] QuestionUpdateDTO questionUpdateDTO)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;

            return await _questionService.HandleUpdateQuestion(questionUpdateDTO, userId);
        }
    }
}
