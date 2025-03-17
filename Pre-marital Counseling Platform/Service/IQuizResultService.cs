using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SWP391.Domain;
using SWP391.DTO;
using SWP391.Infrastructure.DbContext;

namespace SWP391.Service
{
    public interface IQuizResultService
    {
        Task<IActionResult> CreateQuizResult(List<CreateQuizResultDTO> dto, string? userId);
        Task<IActionResult> UpdateQuizResult(UpdateQuizResultDTO dto, string? userId);
        Task<IActionResult> GetAllQuizResult();
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
                    var quizResult = _mapper.Map<QuizResult>(item);
                    quizResult.CreatedBy = Guid.Parse(userId);
                    quizResult.CreatedAt = DateTime.Now;
                    quizResult.UpdatedBy = Guid.Parse(userId);
                    quizResult.UpdatedAt = DateTime.Now;
                    _context.Add(quizResult);
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

        public async Task<IActionResult> GetAllQuizResult()
        {
            try
            {
                var list = _context.QuizResults.ToList();
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
