﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391.Domain;
using SWP391.DTO.Booking;
using SWP391.DTO.Category;
using SWP391.Infrastructure.DataEnum;
using SWP391.Infrastructure.DbContext;

namespace SWP391.Service
{
    public interface IBookingService
    {
        Task<IActionResult> HandleCancelBooking(Guid id, string? userId);
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
                        ScheduleId = x.ScheduleId,
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
                        ScheduleId = x.ScheduleId,
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
                var slot = _context.Schedules.FirstOrDefault(x => x.ScheduleId == bookingCreateDTO.ScheduleId);
                if(slot.IsAvailable == false)
                {
                    return BadRequest("Slot is not available!");
                }

                var userQuery = _context.Users.AsQueryable();

                var member = userQuery.FirstOrDefault(x => x.UserId == bookingCreateDTO.MemberId);
                if(member.Role != UserRoleEnum.MEMBER)
                {
                    return BadRequest("Member unauthorized!");
                }

                if(_context.Therapists.FirstOrDefault(x => x.TherapistId == bookingCreateDTO.TherapistId) == null)
                {
                    return BadRequest("Therapist not found!");
                }

                var bookingQuery = _context.Bookings.AsQueryable();

                if(bookingQuery.FirstOrDefault(e => e.Status == BookingStatusEnum.PENDING && e.ScheduleId == bookingCreateDTO.ScheduleId) == null)
                {
                    return BadRequest("Slot is not available!");
                }

                var booking = new Booking
                {
                    MemberId = bookingCreateDTO.MemberId,
                    TherapistId = bookingCreateDTO.TherapistId,
                    MemberResultId = bookingCreateDTO.MemberResultId,
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
                booking.ScheduleId = bookingUpdateDTO.ScheduleId;

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

        public async Task<IActionResult> HandleCancelBooking(Guid id, string? userId)
        {
            try
            {
                var booking = _context.Bookings
                    .Include(e => e.Schedule)
                    .FirstOrDefault(x => x.BookingId == id);

                if(booking.Status != BookingStatusEnum.PENDING)
                {
                    return BadRequest("Booking is not pending!");
                }

                booking.Status = BookingStatusEnum.CANCELED;
                booking.UpdatedAt = DateTime.Now;
                booking.UpdatedBy = Guid.Parse(userId);
                _context.Bookings.Update(booking);

                var message = "No returned!";

                if ((booking.Schedule.Date - DateTime.Now).TotalHours > 2)
                {
                    var transaction = new Transaction
                    {
                        Amount = +booking.Fee,
                        Description = "Cancel booking",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatedBy = Guid.Parse(userId),
                        UpdatedBy = Guid.Parse(userId)
                    };

                    var transactionQuery = _context.Transactions.AsQueryable();

                    var checkId = true;
                    while (checkId)
                    {
                        Guid transactionId = Guid.NewGuid();
                        var check = transactionQuery.FirstOrDefault(x => x.TransactionId == transactionId);
                        if (check == null)
                        {
                            transaction.TransactionId = transactionId;
                            checkId = false;
                        }
                    }

                    message = "Returned!";
                    _context.Transactions.Add(transaction);
                }

                BookingReturnDTO bookingReturn = new BookingReturnDTO
                {
                    Message = message,
                    Booking = booking
                };

                if (_context.SaveChanges() > 0)
                {
                    return Ok(bookingReturn);
                }
                else
                {
                    return BadRequest("Cancel Booking failed");
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
