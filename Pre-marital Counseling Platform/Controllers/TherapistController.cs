using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391.DTO;
using SWP391.Service;
using System.Security.Claims;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TherapistController : ControllerBase
    {
        private readonly ITherapistService _therapistService;
        private readonly IConfiguration _config;

        public TherapistController(IConfiguration config, ITherapistService therapistService)
        {
            _config = config;
            _therapistService = therapistService;
        }

        [HttpGet("Get_All_Therapists")]
        public async Task<IActionResult> GetAllTherapists()
        {

            return await _therapistService.HandleGetAllTherapists();
        }

        [HttpGet("Get_Therapist_By_Id")]
        public async Task<IActionResult> GetTherapistById([FromQuery] Guid id)
        {

            return await _therapistService.HandleGetTherapistById(id);
        }

        [HttpGet("Get_Therapist_By_SpecificationId")]
        public async Task<IActionResult> GetTherapistBySpecificationId([FromQuery] Guid id)
        {

            return await _therapistService.HandleGetTherapistBySpecificationId(id);
        }

        [HttpGet("Get_Therapist_By_Rating")]
        public async Task<IActionResult> GetTherapistByRating([FromQuery] decimal rating)
        {
            return await _therapistService.GetTherapistByRating(rating);
        }

        [HttpGet("Get_Therapist_By_Name")]
        public async Task<IActionResult> GetTherapistByName([FromQuery] string name)
        {

            return await _therapistService.HandleGetTherapistByName(name);
        }

        [Authorize]
        [HttpPost("Update_Therapist")]
        public async Task<IActionResult> UpdateTherapist([FromBody] TherapistUpdateDTO therapistUpdateDTO)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;

            return await _therapistService.HandleUpdateTherapist(therapistUpdateDTO, userId);
        }
    }
}
