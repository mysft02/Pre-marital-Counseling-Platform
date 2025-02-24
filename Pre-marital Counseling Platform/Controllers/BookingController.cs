using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391.DTO;
using SWP391.DTO.Category;
using SWP391.Service;
using System.Security.Claims;

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

        [Authorize]
        [HttpPost("Create_Booking")]
        public async Task<IActionResult> CreateBooking([FromBody] BookingCreateDTO bookingCreateDTO)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;

            return await _bookingService.HandleCreateBooking(bookingCreateDTO, userId);
        }

        //[Authorize]
        //[HttpPost("Update_Booking")]
        //public async Task<IActionResult> UpdateBooking([FromBody] BookingUpdateDTO bookingUpdateDTO)
        //{
        //    var currentUser = HttpContext.User;
        //    var userId = currentUser.FindFirst("UserId")?.Value;

        //    return await _bookingService.HandleUpdateBooking(bookingUpdateDTO, userId);
        //}

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
