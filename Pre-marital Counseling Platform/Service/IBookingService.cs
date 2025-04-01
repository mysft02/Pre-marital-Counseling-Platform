using AutoMapper;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391.Domain;
using SWP391.DTO;
using SWP391.Infrastructure.DataEnum;
using SWP391.Infrastructure.DbContext;
using static SWP391.Service.BookingService;

namespace SWP391.Service
{
    public interface IBookingService
    {
        Task<IActionResult> HandleCancelBooking(BookingCancelDTO bookingCancelDTO, string? userId);
        Task<IActionResult> HandleCloseBooking(Guid id, string? userId);
        Task<IActionResult> HandleFinishBooking(Guid id, string? userId);
        Task<IActionResult> HandleCreateBooking(BookingCreateDTO bookingCreateDTO, string? userId);
        Task<IActionResult> HandleGetAllBookings();
        Task<IActionResult> HandleGetBookingById(Guid id);
        Task<IActionResult> HandleGetBookingByUserId(Guid userId);
        Task<IActionResult> HandleGetBookingByTherapistId(Guid userId);
        Task<IActionResult> HandleUpdateBooking(BookingUpdateDTO bookingUpdateDTO, string? userId);
        Task<IActionResult> GetCommission();
        Task<IActionResult> UpdateCommission(CommissionDTO commissionDTO);
    }

    public class BookingService : ControllerBase, IBookingService
    {
        private readonly PmcsDbContext _context;
        private readonly IMapper _mapper;

        public BookingService(PmcsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> HandleGetAllBookings()
        {
            try
            {
                List<BookingDTO> bookings = new List<BookingDTO>();
                bookings = _context.Bookings
                    .Include(e => e.Feedback)
                    .Include(e => e.Schedule)
                    .Include(e => e.Therapist)
                    .Select(x => new BookingDTO
                    {
                        BookingId = x.BookingId,
                        MemberId = x.MemberId,
                        TherapistId = x.TherapistId,
                        ScheduleId = x.ScheduleId,
                        Status = x.Status,
                        Feedback = x.Feedback,
                        Schedule = x.Schedule,
                        Therapist = x.Therapist,
                        MeetUrl = x.Therapist.MeetUrl
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
                    .Include(e => e.Feedback)
                    .Include(e => e.Schedule)
                    .Include(e => e.Therapist)
                    .Select(x => new BookingDTO
                    {
                        BookingId = x.BookingId,
                        MemberId = x.MemberId,
                        TherapistId = x.TherapistId,
                        ScheduleId = x.ScheduleId,
                        Status = x.Status,
                        Feedback = x.Feedback,
                        Schedule = x.Schedule,
                        Therapist = x.Therapist,
                        MeetUrl = x.Therapist.MeetUrl
                    })
                    .Where(x => x.BookingId == id && x.Status != BookingStatusEnum.TEST);

                return Ok(booking);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleGetBookingByUserId(Guid id)
        {
            try
            {
                var bookings = _context.Bookings
                    .Include(e => e.Feedback)
                    .Include(e => e.Schedule)
                    .Include(e => e.Therapist)
                    .Select(x => new BookingDTO
                    {
                        BookingId = x.BookingId,
                        MemberId = x.MemberId,
                        TherapistId = x.TherapistId,
                        ScheduleId = x.ScheduleId,
                        Status = x.Status,
                        Feedback = x.Feedback,
                        Schedule = x.Schedule,
                        Therapist = x.Therapist,
                        MeetUrl = x.Therapist.MeetUrl
                    })
                    .Where(x => x.MemberId == id && x.Status != BookingStatusEnum.TEST)
                    .ToList();

                return Ok(bookings);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleGetBookingByTherapistId(Guid id)
        {
            try
            {
                var bookings = _context.Bookings
                    .Include(e => e.Feedback)
                    .Include(e => e.Schedule)
                    .Include(e => e.Therapist)
                    .Select(x => new BookingDTO
                    {
                        BookingId = x.BookingId,
                        MemberId = x.MemberId,
                        TherapistId = x.TherapistId,
                        ScheduleId = x.ScheduleId,
                        Status = x.Status,
                        Feedback = x.Feedback,
                        Schedule = x.Schedule,
                        Therapist = x.Therapist,
                        MeetUrl = x.Therapist.MeetUrl
                    })
                    .Where(x => x.TherapistId == id && x.Status != BookingStatusEnum.TEST)
                    .ToList();

                return Ok(bookings);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleCreateBooking(BookingCreateDTO bookingCreateDTO, string? userId)
        {
            try
            {
                var slot = _context.Schedules.FirstOrDefault(x => x.ScheduleId == bookingCreateDTO.ScheduleId);
                if(slot.Status != ScheduleStatusEnum.Available)
                {
                    return BadRequest("Slot is not available!");
                }

                var userQuery = _context.Users.AsQueryable();

                var member = userQuery.FirstOrDefault(x => x.UserId == bookingCreateDTO.MemberId);
                if(member.Role != UserRoleEnum.MEMBER)
                {
                    return BadRequest("Member unauthorized!");
                }

                var therapist = _context.Therapists.FirstOrDefault(x => x.TherapistId == bookingCreateDTO.TherapistId);
                if(therapist == null)
                {
                    return BadRequest("Therapist not found!");
                }

                var bookingQuery = _context.Bookings.AsQueryable();

                if(bookingQuery.FirstOrDefault(e => e.Status == BookingStatusEnum.PENDING && e.ScheduleId == bookingCreateDTO.ScheduleId) != null)
                {
                    return BadRequest("Slot is not available!");
                }

                var test = bookingQuery.FirstOrDefault(e => e.Status == BookingStatusEnum.TEST);

                var bookingMapped = _mapper.Map<Booking>(bookingCreateDTO);
                bookingMapped.CreatedAt = DateTime.Now;
                bookingMapped.UpdatedAt = DateTime.Now;
                bookingMapped.CreatedBy = Guid.Parse(userId);
                bookingMapped.UpdatedBy = Guid.Parse(userId);
                bookingMapped.Status = BookingStatusEnum.PENDING;
                bookingMapped.Fee = therapist.ConsultationFee;
                bookingMapped.Commission = test.Commission;

                _context.Bookings.Add(bookingMapped);

                slot.Status = ScheduleStatusEnum.Booked;
                _context.Schedules.Update(slot);

                var transaction = new TransactionCreateDTO
                {
                    Amount = -therapist.ConsultationFee,
                    Description = "Order booking",
                };

                var transactionMapped = _mapper.Map<Transaction>(transaction);
                transactionMapped.CreatedAt = DateTime.Now;
                transactionMapped.UpdatedAt = DateTime.Now;
                transactionMapped.CreatedBy = Guid.Parse(userId);
                transactionMapped.UpdatedBy = Guid.Parse(userId);

                _context.Transactions.Add(transactionMapped);

                var wallet = _context.Wallets.FirstOrDefault(c => c.UserId == member.UserId);

                if(wallet.Balance < bookingMapped.Fee)
                {
                    return BadRequest("Balance not enough!");
                }

                wallet.Balance -= therapist.ConsultationFee;
                _context.Wallets.Update(wallet);

                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return Ok(bookingMapped);
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
                var bookingMapped = _mapper.Map<Booking>(bookingUpdateDTO);

                bookingMapped.UpdatedAt = DateTime.Now;
                bookingMapped.UpdatedBy = Guid.Parse(userId);

                _context.Bookings.Update(bookingMapped);
                if (_context.SaveChanges() > 0)
                {
                    return Ok(bookingMapped);
                }
                else
                {
                    return BadRequest("Update failed");
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleCancelBooking(BookingCancelDTO bookingCancelDTO, string? userId)
        {
            try
            {
                var booking = _context.Bookings
                    .Include(e => e.Schedule)
                    .FirstOrDefault(x => x.BookingId == bookingCancelDTO.BookingId);

                if (booking.Status != BookingStatusEnum.PENDING)
                {
                    return BadRequest("Booking is not pending!");
                }

                booking.Status = BookingStatusEnum.CANCELED;
                booking.UpdatedAt = DateTime.Now;
                booking.UpdatedBy = Guid.Parse(userId);
                _context.Bookings.Update(booking);

                if (bookingCancelDTO.IsReturn == true)
                {
                    var transaction = new TransactionDTO
                    {
                        Amount = +booking.Fee,
                        Description = "Cancel booking",
                    };

                    var transactionMapped = _mapper.Map<Transaction>(transaction);
                    transactionMapped.CreatedBy = Guid.Parse(userId);
                    transactionMapped.UpdatedAt = DateTime.Now;
                    transactionMapped.UpdatedBy = Guid.Parse(userId);
                    transactionMapped.CreatedAt = DateTime.Now;

                    _context.Transactions.Add(transactionMapped);

                    var wallet = _context.Wallets.FirstOrDefault(e => e.UserId == booking.MemberId);
                    wallet.Balance += booking.Fee;
                    _context.Wallets.Update(wallet);
                }

                var slot = booking.Schedule;
                slot.Status = ScheduleStatusEnum.Available;
                _context.Schedules.Update(slot);

                if (_context.SaveChanges() > 0)
                {
                    return Ok(booking);
                }
                else
                {
                    return BadRequest("Cancel Booking failed");
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleCloseBooking(Guid id, string? userId)
        {
            try
            {
                var booking = _context.Bookings
                    .Include(e => e.Schedule)
                    .Include(e => e.Feedback)
                    .Include(e => e.BookingResult)
                    .FirstOrDefault(x => x.BookingId == id);

                if (booking.Status != BookingStatusEnum.FINISHED)
                {
                    return BadRequest("Booking is not finished!");
                }

                if(booking.Feedback == null)
                {
                    return BadRequest("No feedback yet!");
                }

                if(booking.BookingResult == null)
                {
                    return BadRequest("No result yet!");
                }

                booking.Status = BookingStatusEnum.CLOSED;
                booking.UpdatedAt = DateTime.Now;
                booking.UpdatedBy = Guid.Parse(userId);
                _context.Bookings.Update(booking);

                var transactionQuery = _context.Transactions.AsQueryable();

                var transaction = new TransactionDTO
                {
                    Amount = (booking.Fee * 75 / 100),
                    Description = "Finish counseling",
                };

                var transactionMapped = _mapper.Map<Transaction>(transaction);
                transactionMapped.CreatedBy = booking.TherapistId;
                transactionMapped.UpdatedAt = DateTime.Now;
                transactionMapped.UpdatedBy = booking.TherapistId;
                transactionMapped.CreatedAt = DateTime.Now;

                _context.Transactions.Add(transactionMapped);

                var wallet = _context.Wallets.FirstOrDefault(c => c.UserId == booking.TherapistId);
                wallet.Balance += ((booking.Fee * (100 - booking.Commission)) / 100);
                _context.Wallets.Update(wallet);

                var adWallet = _context.Wallets.FirstOrDefault(c => c.UserId.ToString() == userId);
                adWallet.Balance += (booking.Fee * 25 / 100);
                _context.Wallets.Update(adWallet);

                var adTransaction = new TransactionCreateDTO
                {
                    Amount = +(booking.Fee * booking.Commission) / 100,
                    Description = "Finish counseling",
                };

                var adTransMapped = _mapper.Map<Transaction>(adTransaction);
                adTransMapped.CreatedBy = Guid.Parse(userId);
                adTransMapped.UpdatedAt = DateTime.Now;
                adTransMapped.UpdatedBy = Guid.Parse(userId);
                adTransMapped.CreatedAt = DateTime.Now;
                _context.Transactions.Add(adTransMapped);

                if (_context.SaveChanges() > 0)
                {
                    return Ok(booking);
                }
                else
                {
                    return BadRequest("Close Booking failed");
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleFinishBooking(Guid id, string? userId)
        {
            try
            {
                var booking = _context.Bookings
                    .Include(e => e.Schedule)
                    .Include(e => e.Feedback)
                    .Include(e => e.BookingResult)
                    .FirstOrDefault(x => x.BookingId == id);

                if (booking.Status != BookingStatusEnum.PENDING)
                {
                    return BadRequest("Booking is not pending!");
                }

                booking.Status = BookingStatusEnum.FINISHED;
                booking.UpdatedAt = DateTime.Now;
                booking.UpdatedBy = Guid.Parse(userId);
                _context.Bookings.Update(booking);

                if (_context.SaveChanges() > 0)
                {
                    return Ok(booking);
                }
                else
                {
                    return BadRequest("Close Booking failed");
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> GetCommission()
        {
            var booking = _context.Bookings.FirstOrDefault(x => x.Status == BookingStatusEnum.TEST);
            var com = booking.Commission;

            var result = new CommissionDTO
            {
                Commission = com,
            };

            return Ok(result);
        }

        public async Task<IActionResult> UpdateCommission(CommissionDTO commissionDTO)
        {
            var booking = _context.Bookings.FirstOrDefault(x => x.Status == BookingStatusEnum.TEST);
            if(commissionDTO.Commission < 0 || commissionDTO.Commission > 100)
            {
                return BadRequest("Commission must be between 0 and 100");
            }
            booking.Commission = commissionDTO.Commission;
            _context.Bookings.Update(booking);
            if (_context.SaveChanges() > 0)
            {
                return Ok(new CommissionDTO { Commission = commissionDTO.Commission });
            }
            else
            {
                return BadRequest("Update commission failed");
            }
        }

        public class CommissionDTO
        {
            public decimal Commission { get; set; }
        }
    }
}
