using Microsoft.AspNetCore.Mvc;
using SWP391.Service;

namespace SWP391.Controllers
{
    [Route("api/Controller")]
    [ApiController]
    public class QuizResultController : ControllerBase
    {
        private readonly IQuizService _service;

        public QuizResultController(IQuizService service)
        {
            _service = service;
        }

        [HttpPost("Create_List_Quiz_Result")]
        public async Task<IActionResult> CreateQuizResult([FromBody] List<CreateListQuizResultDTO> dto)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;
            return await _service.
        }
    }
}
