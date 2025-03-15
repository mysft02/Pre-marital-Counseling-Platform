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

        [HttpGet("Get_All_Quiz_Result")]
        public async Task<IActionResult> GetAllQuizResult()
        {
            return await _service.GetAllQuizResult();
        }

        [Authorize]
        [HttpPost("Create_Quiz_Result")]
        public async Task<IActionResult> CreateQuizResult([FromBody] List<CreateQuizResultDTO> dto)
        {
            if (dto == null || dto.Count < 1 || dto.Count > 4)
            {
                return BadRequest("You must provide between 1 and 4 quiz results.");
            }

            foreach (var item in dto)
            {
                if(item.Level < 1 || item.Level > 4)
                {
                    return BadRequest("Level must be 1, 2, 3 or 4");
                }
            }

            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;
            return await _service.CreateQuizResult(dto, userId);
        }

        [Authorize]
        [HttpPost("Update_Quiz_Result")]
        public async Task<IActionResult> UpdateQuizResult([FromBody] UpdateQuizResultDTO dto)
        {
                if (dto.Level < 1 || dto.Level > 4)
                {
                    return BadRequest("Level must be 1, 2, 3 or 4");
                }
            

            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;
            return await _service.UpdateQuizResult(dto, userId);
        }
    }
}
