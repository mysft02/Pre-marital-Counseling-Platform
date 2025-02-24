using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391.DTO;
using SWP391.Service;
using System.Security.Claims;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IConfiguration _config;

        public FeedbackController(IConfiguration config, IFeedbackService feedbackService)
        {
            _config = config;
            _feedbackService = feedbackService;
        }

        [HttpGet("Get_All_Feedbacks")]
        public async Task<IActionResult> GetAllFeedbacks()
        {

            return await _feedbackService.HandleGetAllFeedbacks();
        }

        [HttpGet("Get_Feedback_By_Id")]
        public async Task<IActionResult> GetFeedbackById([FromQuery] Guid id)
        {

            return await _feedbackService.HandleGetFeedbackById(id);
        }

        [Authorize]
        [HttpPost("Create_Feedback")]
        public async Task<IActionResult> CreateFeedback([FromBody] FeedbackCreateDTO feedbackCreateDTO)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;

            return await _feedbackService.HandleCreateFeedback(feedbackCreateDTO, userId);
        }

        [Authorize]
        [HttpPost("Update_Feedback")]
        public async Task<IActionResult> UpdateFeedback([FromBody] FeedbackUpdateDTO feedbackUpdateDTO)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;

            return await _feedbackService.HandleUpdateFeedback(feedbackUpdateDTO, userId);
        }
    }
}
