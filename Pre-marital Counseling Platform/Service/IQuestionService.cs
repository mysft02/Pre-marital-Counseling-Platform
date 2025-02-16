using Microsoft.AspNetCore.Mvc;
using SWP391.Domain;
using SWP391.DTO.Category;
using SWP391.DTO.Question;
using SWP391.Infrastructure.DataEnum;
using SWP391.Infrastructure.DbContext;

namespace SWP391.Service
{
    public interface IQuestionService
    {
        Task<IActionResult> HandleCreateQuestion(QuestionCreateDTO questionCreateDTO, string? userId);
        Task<IActionResult> HandleGetAllQuestions();
        Task<IActionResult> HandleGetQuestionById(Guid id);
        Task<IActionResult> HandleUpdateQuestion(QuestionUpdateDTO questionUpdateDTO, string? userId);
    }

    public class QuestionService : ControllerBase, IQuestionService
    {
        private readonly PmcsDbContext _context;

        public QuestionService(PmcsDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> HandleGetAllQuestions()
        {
            try
            {
                List<QuestionDTO> questions = new List<QuestionDTO>();
                questions = _context.Questions
                    .Select(x => new QuestionDTO
                    {
                        QuestionId = x.QuestionId,
                        QuizId = x.QuizId,
                        QuestionContent = x.QuestionContent,
                        QuestionStatus = x.Status
                    })
                    .ToList();

                return Ok(questions);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleGetQuestionById(Guid id)
        {
            try
            {
                var question = _context.Questions
                    .Select(x => new QuestionDTO
                    {
                        QuestionId = x.QuestionId,
                        QuizId = x.QuizId,
                        QuestionContent = x.QuestionContent,
                        QuestionStatus = x.Status
                    })
                    .Where(x => x.QuestionId == id);

                return Ok(question);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleCreateQuestion(QuestionCreateDTO questionCreateDTO, string? userId)
        {
            try
            {
                var Question = new Question
                {
                    QuestionContent = questionCreateDTO.QuestionContent,
                    QuizId = questionCreateDTO.QuizId,
                    Status = QuestionStatusEnum.ACTIVE,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = Guid.Parse(userId),
                    UpdatedBy = Guid.Parse(userId)
                };

                var check = true;
                while (check)
                {
                    var id = Guid.NewGuid();
                    var checkId = _context.Questions.FirstOrDefault(x => x.QuestionId == id);
                    if (checkId == null)
                    {
                        Question.QuestionId = id;
                        check = false;
                    }
                }

                _context.Questions.Add(Question);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return Ok(Question);
                }
                else
                {
                    return BadRequest("Create failed");
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleUpdateQuestion(QuestionUpdateDTO questionUpdateDTO, string? userId)
        {
            try
            {
                var question = _context.Questions.FirstOrDefault(x => x.QuestionId == questionUpdateDTO.QuestionId);

                question.QuestionContent = questionUpdateDTO.QuestionContent;
                question.QuizId = questionUpdateDTO.QuizId;
                question.Status = (QuestionStatusEnum)questionUpdateDTO.QuestionStatus;
                question.UpdatedAt = DateTime.Now;
                question.UpdatedBy = Guid.Parse(userId);

                _context.Questions.Update(question);
                if (_context.SaveChanges() > 0)
                {
                    return Ok(question);
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
