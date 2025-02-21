using Microsoft.AspNetCore.Mvc;
using SWP391.Domain;
using SWP391.DTO;
using SWP391.DTO.BookingResult;
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

        public BookingResultService(PmcsDbContext context)
        {
            _context = context;
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
                var bookingResult = new BookingResult
                {
                    BookingId = bookingResultCreateDTO.BookingId,
                    Description = bookingResultCreateDTO.Description,
                };

                var check = true;
                while (check)
                {
                    var id = Guid.NewGuid();
                    var checkId = _context.BookingResults.FirstOrDefault(x => x.BookingResultId == id);
                    if (checkId == null)
                    {
                        bookingResult.BookingResultId = id;
                        check = false;
                    }
                }

                _context.BookingResults.Add(bookingResult);
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
