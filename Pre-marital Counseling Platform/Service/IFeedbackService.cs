﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391.Domain;
using SWP391.DTO;
using SWP391.Infrastructure.DataEnum;
using SWP391.Infrastructure.DbContext;

namespace SWP391.Service
{
    public interface IFeedbackService
    {
        Task<IActionResult> HandleCreateFeedback(FeedbackCreateDTO feedbackCreateDTO, string? userId);
        Task<IActionResult> HandleGetAllFeedbacks();
        Task<IActionResult> HandleGetFeedbackById(Guid id);
        Task<IActionResult> HandleGetFeedbackByTherapistId(Guid id);
        Task<IActionResult> HandleUpdateFeedback(FeedbackUpdateDTO feedbackUpdateDTO, string? userId);
    }

    public class FeedbackService : ControllerBase, IFeedbackService
    {
        private readonly PmcsDbContext _context;
        private readonly IMapper _mapper;

        public FeedbackService(PmcsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> HandleCreateFeedback(FeedbackCreateDTO feedbackCreateDTO, string? userId)
        {
            try
            {
                var bookingCheck = _context.Bookings
                    .FirstOrDefault(x => x.BookingId == feedbackCreateDTO.BookingId && x.Status == BookingStatusEnum.FINISHED);

                if(bookingCheck == null) { return BadRequest("Booking not finished"); }

                var feedback = new FeedbackDTO
                {
                    FeedbackTitle = feedbackCreateDTO.FeedbackTitle,
                    FeedbackContent = feedbackCreateDTO.FeedbackContent,
                    IsSatisfied = feedbackCreateDTO.IsSatisfied,
                    BookingId = feedbackCreateDTO.BookingId,
                    Rating = feedbackCreateDTO.Rating,
                };

                var feedbackMapped = _mapper.Map<Feedback>(feedback);
                feedbackMapped.CreatedBy = Guid.Parse(userId);
                feedbackMapped.CreatedAt = DateTime.Now;
                feedbackMapped.UpdatedBy = Guid.Parse(userId);
                feedbackMapped.UpdatedAt = DateTime.Now;

                _context.Feedbacks.Add(feedbackMapped);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return Ok(feedback);
                }
                else
                {
                    return BadRequest("Create failed");
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleGetAllFeedbacks()
        {
            try
            {
                List<FeedBackReturnDTO> feedbacks = new List<FeedBackReturnDTO>();
                feedbacks = _context.Feedbacks
                    .Include(x => x.Booking).ThenInclude(x => x.User)
                    .Select(x => new FeedBackReturnDTO
                    {
                        UserName = x.Booking.User.FullName,
                        AvatarUrl = x.Booking.User.AvatarUrl,
                        FeedbackId = x.FeedbackId,
                        FeedbackTitle = x.FeedbackTitle,
                        FeedbackContent = x.FeedbackContent,
                        IsSatisfied = x.IsSatisfied,
                        BookingId = x.BookingId,
                        Rating = x.Rating,
                    })
                    .ToList();

                return Ok(feedbacks);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleGetFeedbackById(Guid id)
        {
            try
            {
                var feedback = _context.Feedbacks
                    .Include(x => x.Booking).ThenInclude(x => x.User)
                    .Select(x => new FeedBackReturnDTO
                    {
                        UserName = x.Booking.User.FullName,
                        AvatarUrl = x.Booking.User.AvatarUrl,
                        FeedbackId = x.FeedbackId,
                        FeedbackTitle = x.FeedbackTitle,
                        FeedbackContent = x.FeedbackContent,
                        IsSatisfied = x.IsSatisfied,
                        BookingId = x.BookingId,
                        Rating = x.Rating,
                    })
                    .Where(c => c.FeedbackId == id);

                return Ok(feedback);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleGetFeedbackByTherapistId(Guid id)
        {
            try
            {
                var feedback = _context.Feedbacks
                    .Where(c => c.Booking.TherapistId == id)
                    .Include(x => x.Booking).ThenInclude(x => x.User)
                    .Select(x => new FeedBackReturnDTO
                    {
                        UserName = x.Booking.User.FullName,
                        AvatarUrl = x.Booking.User.AvatarUrl,
                        FeedbackId = x.FeedbackId,
                        FeedbackTitle = x.FeedbackTitle,
                        FeedbackContent = x.FeedbackContent,
                        IsSatisfied = x.IsSatisfied,
                        BookingId = x.BookingId,
                        Rating = x.Rating,
                    });

                return Ok(feedback);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleUpdateFeedback(FeedbackUpdateDTO feedbackUpdateDTO, string? userId)
        {
            try
            {
                var feedback = _context.Feedbacks.FirstOrDefault(x => x.FeedbackId == feedbackUpdateDTO.FeedbackId);

                feedback.FeedbackTitle = feedbackUpdateDTO.FeedbackTitle;
                feedback.FeedbackContent = feedbackUpdateDTO.FeedbackContent;
                feedback.IsSatisfied = feedbackUpdateDTO.IsSatisfied;

                _context.Feedbacks.Update(feedback);
                if (_context.SaveChanges() > 0)
                {
                    return Ok(feedback);
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
