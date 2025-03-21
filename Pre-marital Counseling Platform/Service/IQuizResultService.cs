using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SWP391.Domain;
using SWP391.DTO;
using SWP391.Infrastructure.DataEnum;
using SWP391.Infrastructure.DbContext;

namespace SWP391.Service
{
    public interface IQuizResultService
    {
        Task<IActionResult> CreateQuizResult(List<CreateQuizResultDTO> dto, string? userId);
        Task<IActionResult> UpdateQuizResult(UpdateQuizResultDTO dto, string? userId);
        Task<IActionResult> GetAllQuizResult();
        Task<IActionResult> DeleteQuizResult(Guid id, string? userId);
    }

    public class QuizResultService : ControllerBase, IQuizResultService
    {

        private readonly IMapper _mapper;
        private readonly PmcsDbContext _context;

        public QuizResultService(IMapper mapper, PmcsDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IActionResult> CreateQuizResult(List<CreateQuizResultDTO> dto, string? userId)
        {
            try
            {
                foreach (var item in dto)
                {
                    var duplicate = _context.QuizResults
                        .FirstOrDefault(x => x.QuizId == item.QuizId && x.Level == item.Level
                                             && x.Title == item.Title);

                    if(duplicate == null)
                    {
                        var quizResult = _mapper.Map<QuizResult>(item);
                        quizResult.Score = quizResult.Level * 25;
                        quizResult.CreatedBy = Guid.Parse(userId);
                        quizResult.CreatedAt = DateTime.Now;
                        quizResult.UpdatedBy = Guid.Parse(userId);
                        quizResult.UpdatedAt = DateTime.Now;
                        _context.Add(quizResult);
                    }
                    else
                    {
                        return BadRequest("Duplicate data" + item.Title + " " + item.Level);
                    }
                }

                var rs = await _context.SaveChangesAsync();
                if (rs > 0)
                {
                    return Ok("Create successfully");
                }
                else
                {
                    return BadRequest("Create failed");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> DeleteQuizResult(Guid id, string? userId)
        {
            try
            {
                var quizResult = _context.QuizResults.FirstOrDefault(x => x.QuizResultId == id);
                if (quizResult == null)
                {
                    return NotFound("Quiz Result not found.");
                }

                quizResult.Status = QuizResultStatusEnum.Inactive;

                _context.QuizResults.Update(quizResult);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return Ok("Quiz Result deleted successfully.");
                }
                else
                {
                    return BadRequest("Failed to delete Quiz Result.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> GetAllQuizResult()
        {
            try
            {
                var list = _context.QuizResults
                    .Where(x => x.Status == QuizResultStatusEnum.Active)
                    .ToList();
                return Ok(list);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> UpdateQuizResult(UpdateQuizResultDTO dto, string? userId)
        {
            try
            {
                var quizResult = _mapper.Map<QuizResult>(dto);
                quizResult.Score = quizResult.Level * 25;
                quizResult.UpdatedBy = Guid.Parse(userId);
                quizResult.UpdatedAt = DateTime.Now;
                _context.QuizResults.Update(quizResult);
                if (_context.SaveChanges() > 0)
                {
                    return Ok("Update successfully");
                }
                else
                {
                    return BadRequest("Update failed");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
