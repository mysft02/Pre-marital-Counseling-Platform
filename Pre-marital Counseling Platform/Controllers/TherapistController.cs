using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391.DTO.Booking;
using SWP391.DTO.Therapist;
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

        //[Authorize]
        //[HttpPost("Create_Therapist")]
        //public async Task<IActionResult> CreateTherapist([FromBody] TherapistCreateDTO therapistCreateDTO)
        //{
        //    var currentUser = HttpContext.User;
        //    var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    return await _therapistService.HandleCreateTherapist(therapistCreateDTO, userId);
        //}
    }
}
