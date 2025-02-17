using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SWP391.Domain;
using SWP391.DTO.Quiz;
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
    }

    public class QuizService : ControllerBase, IQuizService
    {
        private readonly PmcsDbContext _context;

        public QuizService(PmcsDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> HandleGetAllQuizzes()
        {
            try
            {
                var Quizzes = _context.Quizes
                    .Include(x => x.Category)
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
                    .FirstOrDefault();

                return Ok(Quiz);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleCreateQuiz(QuizCreateDTO quizCreateDTO, string userId)
        {
            try
            {
                var quiz = new Quiz
                {
                    CategoryId = quizCreateDTO.CategoryId,
                    Name = quizCreateDTO.Name,
                    Description = quizCreateDTO.Description,
                    QuizStatus = QuizStatusEnum.ACTIVE, 
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = Guid.Parse(userId),
                    UpdatedBy = Guid.Parse(userId)
                };

                var check = true;
                while (check)
                {
                    var id = Guid.NewGuid();
                    var checkId = _context.Quizes.FirstOrDefault(x => x.QuizId == id);
                    if (checkId == null)
                    {
                        quiz.QuizId = id;
                        check = false;
                    }
                }

                _context.Quizes.Add(quiz);
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
    }
}
