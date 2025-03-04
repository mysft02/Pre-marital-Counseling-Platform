using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SWP391.Domain;
using SWP391.DTO;
using SWP391.Infrastructure.DataEnum;
using SWP391.Infrastructure.DbContext;

namespace SWP391.Service
{
    public interface IBookingResultService
    {
        Task<IActionResult> HandleCreateBookingResult(BookingResultCreateDTO bookingResultCreateDTO, string? userId);
        Task<IActionResult> HandleGetAllBookingResults();
        Task<IActionResult> HandleGetBookingResultById(Guid id);
        //Task<IActionResult> HandleUpdateBookingResult(BookingResultUpdateDTO bookingResultUpdateDTO, string? userId);
    }

    public class BookingResultService : ControllerBase, IBookingResultService
    {
        private readonly PmcsDbContext _context;
        private readonly IMapper _mapper;

        public BookingResultService(PmcsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> HandleGetAllBookingResults()
        {
            try
            {
                List<BookingResultDTO> bookingResults = new List<BookingResultDTO>();
                bookingResults = _context.BookingResults
                    .Select(x => new BookingResultDTO
                    {
                        BookingResultId = x.BookingResultId,
                        BookingId = x.BookingResultId,
                        Description = x.Description,
                    })
                    .ToList();

                return Ok(bookingResults);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleGetBookingResultById(Guid id)
        {
            try
            {
                var bookingResult = _context.BookingResults
                    .Select(x => new BookingResultDTO
                    {
                        BookingResultId = x.BookingResultId,
                        BookingId = x.BookingResultId,
                        Description = x.Description,
                    })
                    .Where(x => x.BookingResultId == id);

                return Ok(bookingResult);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleCreateBookingResult(BookingResultCreateDTO bookingResultCreateDTO, string? userId)
        {
            try
            {
                var bookingCheck = _context.Bookings.FirstOrDefault(x => x.BookingId == bookingResultCreateDTO.BookingId && x.Status == BookingStatusEnum.FINISHED);

                if(bookingCheck == null)
                {
                    return BadRequest("Booking not finished");
                }
                
                var bookingResult = new BookingResultCreateDTO
                {
                    BookingId = bookingResultCreateDTO.BookingId,
                    Description = bookingResultCreateDTO.Description,
                };

                var bookingResultMapped = _mapper.Map<BookingResult>(bookingResultCreateDTO);

                _context.BookingResults.Add(bookingResultMapped);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return Ok(bookingResult);
                }
                else
                {
                    return BadRequest("Create failed");
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
