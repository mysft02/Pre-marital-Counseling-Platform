using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SWP391.Domain;
using SWP391.DTO;
using SWP391.Infrastructure.DataEnum;
using SWP391.Infrastructure.DbContext;
using System.Collections;

namespace SWP391.Service
{
    public interface IQuizService
    {
        Task<IActionResult> HandleGetAllQuizzes();
        Task<IActionResult> HandleGetQuizById(Guid id);
        Task<IActionResult> HandleCreateQuiz(QuizCreateDTO quizCreateDTO, string userId);
        Task<IActionResult> HandleUpdateQuiz(QuizUpdateDTO quizUpdateDTO, string userId);
        Task<IActionResult> HandleDisableQuiz(Guid id);
    }

    public class QuizService : ControllerBase, IQuizService
    {
        private readonly PmcsDbContext _context;
        private readonly IMapper _mapper;

        public QuizService(PmcsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> HandleGetAllQuizzes()
        {
            try
            {
                var Quizzes = _context.Quizes
                    .Include(x => x.Category)
                    .Include(x => x.Questions).ThenInclude(x => x.Answers)
                    .Include(x => x.QuizResults)
                    .ToList();

                return Ok(Quizzes);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleGetQuizById(Guid id)
        {
            try
            {
                var Quiz = _context.Quizes
                    .Where(x => x.QuizId == id)
                    .Include(x => x.Category)
                    .Include(x => x.Questions).ThenInclude(x => x.Answers)
                    .Include(x => x.QuizResults)
                    .FirstOrDefault();

                return Ok(Quiz);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleCreateQuiz(QuizCreateDTO quizCreateDTO, string userId)
        {
            try
            {
                var quiz = new QuizCreateDTO
                {
                    CategoryId = quizCreateDTO.CategoryId,
                    Name = quizCreateDTO.Name,
                    Description = quizCreateDTO.Description,
                };

                var quizMapped = _mapper.Map<Quiz>(quiz);
                quizMapped.CreatedAt = DateTime.Now;
                quizMapped.CreatedBy = Guid.Parse(userId);
                quizMapped.UpdatedAt = DateTime.Now;
                quizMapped.UpdatedBy = Guid.Parse(userId);
                quizMapped.QuizStatus = QuizStatusEnum.ACTIVE;
                _context.Quizes.Add(quizMapped);
                
                var result = _context.SaveChanges();
                if(result > 0)
                {
                    return Ok(quiz);
                }
                else
                {
                    return BadRequest("Create failed");
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleUpdateQuiz(QuizUpdateDTO quizUpdateDTO, string userId)
        {
            try
            {
                var quiz = _context.Quizes.FirstOrDefault(x => x.QuizId == quizUpdateDTO.QuizId);

                quiz.CategoryId = quizUpdateDTO.CategoryId;
                quiz.Name = quizUpdateDTO.Name;
                quiz.Description = quizUpdateDTO.Description;
                quiz.QuizStatus = (QuizStatusEnum)quizUpdateDTO.Status;
                quiz.UpdatedAt = DateTime.Now;
                quiz.UpdatedBy = Guid.Parse(userId);

                _context.Quizes.Update(quiz);
                if(_context.SaveChanges() > 0)
                {
                    return Ok(quiz);
                }
                else
                {
                    return BadRequest("Update failed");
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleDisableQuiz(Guid id)
        {
            try
            {
                var quiz = _context.Quizes.FirstOrDefault(x => x.QuizId == id);

                quiz.QuizStatus = QuizStatusEnum.INACTIVE;

                _context.Quizes.Update(quiz);
                if (_context.SaveChanges() > 0)
                {
                    return Ok(quiz);
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
