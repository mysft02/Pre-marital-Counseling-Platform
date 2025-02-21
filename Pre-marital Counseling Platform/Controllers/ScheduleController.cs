using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391.DTO;
using SWP391.Infrastructure.DataEnum;
using SWP391.Service;
using System.Security.Claims;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        private readonly IConfiguration _config;

        public ScheduleController(IConfiguration config, IScheduleService scheduleService)
        {
            _config = config;
            _scheduleService = scheduleService;
        }

        [HttpGet("Get_All_Schedules")]
        public async Task<IActionResult> GetAllSchedules()
        {

            return await _scheduleService.HandleGetAllSchedules();
        }

        [HttpGet("Get_Schedule_By_Id")]
        public async Task<IActionResult> GetScheduleById([FromQuery] Guid id)
        {

            return await _scheduleService.HandleGetScheduleById(id);
        }

        [Authorize]
        [HttpGet("Get_Schedule_By_TherapistId")]
        public async Task<IActionResult> GetScheduleByTherapistId()
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;
            var userRole = currentUser.FindFirst("Role")?.Value;

            if (userRole != UserRoleEnum.THERAPIST.ToString())
            {
                return Unauthorized("User is not a therapist!");
            }

            return await _scheduleService.HandleGetScheduleByTherapistId(Guid.Parse(userId));
        }

        [Authorize]
        [HttpPost("Create_Schedule")]
        public async Task<IActionResult> CreateSchedule([FromBody] List<ScheduleCreateDTO> scheduleCreateDTO)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;

            return await _scheduleService.HandleCreateSchedule(scheduleCreateDTO, userId);
        }

        [Authorize]
        [HttpPost("Update_Schedule")]
        public async Task<IActionResult> UpdateSchedule([FromBody] List<ScheduleUpdateDTO> scheduleUpdateDTO)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;

            return await _scheduleService.HandleUpdateSchedule(scheduleUpdateDTO, userId);
        }
    }
}
