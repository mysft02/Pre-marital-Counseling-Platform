using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391.DTO.Booking;
using SWP391.DTO.BookingResult;
using SWP391.Service;
using System.Security.Claims;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingResultController : ControllerBase
    {
        private readonly IBookingResultService _bookingResultService;
        private readonly IConfiguration _config;

        public BookingResultController(IConfiguration config, IBookingResultService bookingResultService)
        {
            _config = config;
            _bookingResultService = bookingResultService;
        }

        [HttpGet("Get_All_Booking_Results")]
        public async Task<IActionResult> GetAllBookingResults()
        {

            return await _bookingResultService.HandleGetAllBookingResults();
        }

        [HttpGet("Get_Booking_Result_By_Id")]
        public async Task<IActionResult> GetBookingResultById([FromQuery] Guid id)
        {

            return await _bookingResultService.HandleGetBookingResultById(id);
        }

        [Authorize]
        [HttpPost("Create_Booking_Result")]
        public async Task<IActionResult> CreateBookingResult([FromBody] BookingResultCreateDTO bookingResultCreateDTO)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bookingResultService.HandleCreateBookingResult(bookingResultCreateDTO, userId);
        }

        //[Authorize]
        //[HttpPost("Update_Booking_Result")]
        //public async Task<IActionResult> UpdateBookingResult([FromBody] BookingResultUpdateDTO bookingResultUpdateDTO)
        //{
        //    var currentUser = HttpContext.User;
        //    var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    return await _bookingResultService.HandleUpdateBookingResult(bookingResultUpdateDTO, userId);
        //}
    }
}
