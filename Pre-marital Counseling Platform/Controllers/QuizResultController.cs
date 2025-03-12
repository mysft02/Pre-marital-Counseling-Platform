using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391.DTO;
using SWP391.Service;

namespace SWP391.Controllers
{
    [Route("api/Controller")]
    [ApiController]
    public class QuizResultController : ControllerBase
    {
        private readonly IQuizResultService _service;

        public QuizResultController(IQuizResultService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost("Create_Quiz_Result")]
        public async Task<IActionResult> CreateQuizResult([FromBody] List<CreateQuizResultDTO> dto)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;
            return await _service.CreateQuizResult(dto, userId);
        }

        [Authorize]
        [HttpPost("Update_Quiz_Result")]
        public async Task<IActionResult> UpdateQuizResult([FromBody] CreateQuizResultDTO dto)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;
            return await _service.UpdateQuizResult(dto, userId);
        }
    }
}
