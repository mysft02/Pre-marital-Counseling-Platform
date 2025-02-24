using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SWP391.Domain;
using SWP391.DTO;
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
        private IMapper _mapper;

        public QuestionService(PmcsDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
                var question = new QuestionDTO
                {
                    QuestionContent = questionCreateDTO.QuestionContent,
                    QuizId = questionCreateDTO.QuizId,
                };

                var questionMapped = _mapper.Map<Question>(question);
                questionMapped.Status = QuestionStatusEnum.ACTIVE;
                questionMapped.CreatedAt = DateTime.Now;
                questionMapped.CreatedBy = Guid.Parse(userId);
                questionMapped.UpdatedAt = DateTime.Now;
                questionMapped.UpdatedBy = Guid.Parse(userId);

                _context.Questions.Add(questionMapped);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return Ok(questionMapped);
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
