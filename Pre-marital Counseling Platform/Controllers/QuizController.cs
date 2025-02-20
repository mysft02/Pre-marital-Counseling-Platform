using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391.DTO.Quiz;
using SWP391.DTO.User;
using SWP391.Service;
using System.Security.Claims;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;
        private readonly IConfiguration _config;

        public QuizController(IConfiguration config, IQuizService quizService)
        {
            _config = config;
            _quizService = quizService;
        }

        [HttpGet("Get_All_Quiz")]
        public async Task<IActionResult> GetAllQuiz()
        {

            return await _quizService.HandleGetAllQuizzes();
        }

        [HttpGet("Get_Quiz_By_Id")]
        public async Task<IActionResult> GetQuizById([FromQuery] Guid id)
        {

            return await _quizService.HandleGetQuizById(id);
        }

        [Authorize]
        [HttpPost("Create_Quiz")]
        public async Task<IActionResult> CreateQuiz([FromBody] QuizCreateDTO quizCreateDTO)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst(ClaimTypes.Sid)?.Value;

            return await _quizService.HandleCreateQuiz(quizCreateDTO, userId);
        }

        [Authorize]
        [HttpPost("Update_Quiz")]
        public async Task<IActionResult> UpdateQuiz([FromBody] QuizUpdateDTO quizUpdateDTO)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst(ClaimTypes.Sid)?.Value;

            return await _quizService.HandleUpdateQuiz(quizUpdateDTO, userId);
        }
    }
}
