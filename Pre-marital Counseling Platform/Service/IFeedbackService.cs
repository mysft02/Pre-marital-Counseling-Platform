using Microsoft.AspNetCore.Mvc;
using SWP391.Domain;
using SWP391.DTO;
using SWP391.DTO.Category;
using SWP391.DTO.Feedback;
using SWP391.Infrastructure.DataEnum;
using SWP391.Infrastructure.DbContext;

namespace SWP391.Service
{
    public interface IFeedbackService
    {
        Task<IActionResult> HandleCreateFeedback(FeedbackCreateDTO feedbackCreateDTO, string? userId);
        Task<IActionResult> HandleGetAllFeedbacks();
        Task<IActionResult> HandleGetFeedbackById(Guid id);
        Task<IActionResult> HandleUpdateFeedback(FeedbackUpdateDTO feedbackUpdateDTO, string? userId);
    }

    public class FeedbackService : ControllerBase, IFeedbackService
    {
        private readonly PmcsDbContext _context;

        public FeedbackService(PmcsDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> HandleCreateFeedback(FeedbackCreateDTO feedbackCreateDTO, string? userId)
        {
            try
            {
                var feedback = new Feedback
                {
                    FeedbackTitle = feedbackCreateDTO.FeedbackTitle,
                    FeedbackContent = feedbackCreateDTO.FeedbackContent,
                    IsSatisfied = feedbackCreateDTO.IsSatisfied,
                    BookingId = feedbackCreateDTO.BookingId,
                    Rating = feedbackCreateDTO.Rating,
                };

                var check = true;
                while (check)
                {
                    var id = Guid.NewGuid();
                    var checkId = _context.Feedbacks.FirstOrDefault(x => x.FeedbackId == id);
                    if (checkId == null)
                    {
                        feedback.FeedbackId = id;
                        check = false;
                    }
                }

                _context.Feedbacks.Add(feedback);
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
                List<FeedbackDTO> feedbacks = new List<FeedbackDTO>();
                feedbacks = _context.Feedbacks
                    .Select(x => new FeedbackDTO
                    {
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
                    .Select(x => new FeedbackDTO
                    {
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
