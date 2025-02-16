using Microsoft.AspNetCore.Mvc;
using SWP391.Domain;
using SWP391.DTO.Booking;
using SWP391.DTO.Category;
using SWP391.Infrastructure.DataEnum;
using SWP391.Infrastructure.DbContext;

namespace SWP391.Service
{
    public interface IBookingService
    {
        Task<IActionResult> HandleCreateBooking(BookingCreateDTO bookingCreateDTO, string? userId);
        Task<IActionResult> HandleGetAllBookings();
        Task<IActionResult> HandleGetBookingById(Guid id);
        Task<IActionResult> HandleUpdateBooking(BookingUpdateDTO bookingUpdateDTO, string? userId);
    }

    public class BookingService : ControllerBase, IBookingService
    {
        private readonly PmcsDbContext _context;

        public BookingService(PmcsDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> HandleGetAllBookings()
        {
            try
            {
                List<BookingDTO> bookings = new List<BookingDTO>();
                bookings = _context.Bookings
                    .Select(x => new BookingDTO
                    {
                        BookingId = x.BookingId,
                        MemberId = x.MemberId,
                        TherapistId = x.TherapistId,
                        MemberResultId = x.MemberResultId,
                        SlotId = x.SlotId,
                        Status = x.Status
                    })
                    .ToList();

                return Ok(bookings);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleGetBookingById(Guid id)
        {
            try
            {
                var booking = _context.Bookings
                    .Select(x => new BookingDTO
                    {
                        BookingId = x.BookingId,
                        MemberId = x.MemberId,
                        TherapistId = x.TherapistId,
                        MemberResultId = x.MemberResultId,
                        SlotId = x.SlotId,
                        Status = x.Status
                    })
                    .Where(x => x.BookingId == id);

                return Ok(booking);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleCreateBooking(BookingCreateDTO bookingCreateDTO, string? userId)
        {
            try
            {
                var booking = new Booking
                {
                    MemberId = bookingCreateDTO.MemberId,
                    TherapistId = bookingCreateDTO.TherapistId,
                    MemberResultId = bookingCreateDTO.MemberResultId,
                    SlotId = bookingCreateDTO.SlotId,
                    Status = BookingStatusEnum.PENDING,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = Guid.Parse(userId),
                    UpdatedBy = Guid.Parse(userId)
                };

                var check = true;
                while (check)
                {
                    var id = Guid.NewGuid();
                    var checkId = _context.Bookings.FirstOrDefault(x => x.BookingId == id);
                    if (checkId == null)
                    {
                        booking.BookingId = id;
                        check = false;
                    }
                }

                _context.Bookings.Add(booking);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return Ok(booking);
                }
                else
                {
                    return BadRequest("Create failed");
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleUpdateBooking(BookingUpdateDTO bookingUpdateDTO, string? userId)
        {
            try
            {
                var booking = _context.Bookings.FirstOrDefault(x => x.BookingId == bookingUpdateDTO.BookingId);

                booking.Status = (BookingStatusEnum)bookingUpdateDTO.Status;
                booking.UpdatedAt = DateTime.Now;
                booking.UpdatedBy = Guid.Parse(userId);
                booking.MemberId = bookingUpdateDTO.MemberId;
                booking.TherapistId = bookingUpdateDTO.TherapistId;
                booking.MemberResultId = bookingUpdateDTO.MemberResultId;
                booking.SlotId = bookingUpdateDTO.SlotId;

                _context.Bookings.Update(booking);
                if (_context.SaveChanges() > 0)
                {
                    return Ok(booking);
                }
                else
                {
                    return BadRequest("Update failed");
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
