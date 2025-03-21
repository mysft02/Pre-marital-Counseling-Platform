using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391.DTO;
using SWP391.Service;
using System.Security.Claims;
using static SWP391.Service.BookingService;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IConfiguration _config;

        public BookingController(IConfiguration config, IBookingService bookingService)
        {
            _config = config;
            _bookingService = bookingService;
        }

        [HttpGet("Get_All_Bookings")]
        public async Task<IActionResult> GetAllBookings()
        {

            return await _bookingService.HandleGetAllBookings();
        }

        [HttpGet("Get_Booking_By_Id")]
        public async Task<IActionResult> GetBookingById([FromQuery] Guid id)
        {

            return await _bookingService.HandleGetBookingById(id);
        }

        [HttpGet("Get_Booking_By_User_Id")]
        public async Task<IActionResult> GetBookingByUserId([FromQuery] Guid id)
        {

            return await _bookingService.HandleGetBookingByUserId(id);
        }

        [HttpGet("Get_Booking_By_Therapist_Id")]
        public async Task<IActionResult> GetBookingByTherapistId([FromQuery] Guid id)
        {

            return await _bookingService.HandleGetBookingByTherapistId(id);
        }

        [HttpGet("Get_Commission")]
        public async Task<IActionResult> GetCommission()
        {

            return await _bookingService.GetCommission();
        }

        [HttpPost("Update_Commission")]
        public async Task<IActionResult> UpdateCommission(CommissionDTO commissionDTO)
        {

            return await _bookingService.UpdateCommission(commissionDTO);
        }

        [Authorize]
        [HttpPost("Create_Booking")]
        public async Task<IActionResult> CreateBooking([FromBody] BookingCreateDTO bookingCreateDTO)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;

            return await _bookingService.HandleCreateBooking(bookingCreateDTO, userId);
        }

        [Authorize]
        [HttpPost("Finish_Booking")]
        public async Task<IActionResult> FinishBooking([FromQuery] Guid id)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;

            return await _bookingService.HandleFinishBooking(id, userId);
        }

        [Authorize]
        [HttpPost("Cancel_Booking")]
        public async Task<IActionResult> CancelBooking([FromQuery] Guid id)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;

            return await _bookingService.HandleCancelBooking(id, userId);
        }

        [Authorize]
        [HttpPost("Close_Booking")]
        public async Task<IActionResult> CloseBooking([FromQuery] Guid id)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;

            return await _bookingService.HandleCloseBooking(id, userId);
        }
    }
}
