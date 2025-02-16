using Microsoft.AspNetCore.Mvc;
using SWP391.Domain;
using SWP391.DTO.Answer;
using SWP391.Infrastructure.DbContext;

namespace SWP391.Service
{
    public interface IAnswerService
    {
        Task<IActionResult> GetAllAnswer();
        Task<IActionResult> GetAnswerById(Guid id);
        Task<IActionResult> CreateAnswer(CreateAnswerDTO dto, string? userId);
        Task<IActionResult> UpdateAnswer(UpdateAnswerDTO dto, string? userId);
    }

    public class AnswerSevice : ControllerBase, IAnswerService
    {
        private readonly PmcsDbContext _context;

        public AnswerSevice(PmcsDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetAllAnswer()
        {
            try
            {
                List<AnswerDTO> answerList = _context.Answers.Select(x => new AnswerDTO
                {
                    AnswerId = x.AnswerId,
                    QuestionId = x.QuestionId,
                    AnswerContent = x.AnswerContent,
                    Score = x.Score,
                }).ToList();

                return Ok(answerList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> GetAnswerById(Guid id)
        {
            try
            {
                var answer = _context.Answers.Where(x => x.AnswerId == id).Select(x => new AnswerDTO
                {
                    AnswerId = x.AnswerId,
                    QuestionId = x.QuestionId,
                    AnswerContent = x.AnswerContent,
                    Score = x.Score,
                });

                if (answer == null)
                {
                    return NotFound();
                }

                return Ok(answer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> CreateAnswer(CreateAnswerDTO dto, string? userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User id required");
                }

                var answer = new Answer
                {
                    QuestionId = dto.QuestionId,
                    AnswerContent = dto.AnswerContent,
                    Score = dto.Score,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = Guid.Parse(userId),
                    UpdatedBy = Guid.Parse(userId)
                };

                var check = true;
                while (check)
                {
                    var id = Guid.NewGuid();
                    var checkId = _context.Answers.FirstOrDefault(x => x.AnswerId == id);
                    if (checkId == null)
                    {
                        answer.AnswerId = id;
                        check = false;
                    }
                }

                _context.Add(answer);
                var save = _context.SaveChanges();
                if (save > 0)
                {
                    return Ok(answer);
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

        public async Task<IActionResult> UpdateAnswer(UpdateAnswerDTO dto, string userId)
        {
            try
            {
                var answer = _context.Answers.FirstOrDefault(x => x.AnswerId == dto.AnswerId);

                answer.AnswerId = dto.AnswerId;
                answer.AnswerContent = dto.AnswerContent;
                answer.AnswerContent = dto.AnswerContent;
                answer.Score = dto.Score;
                answer.UpdatedAt = DateTime.Now;
                answer.UpdatedBy = Guid.Parse(userId);

                _context.Update(answer);
                if (_context.SaveChanges() > 0)
                {
                    return Ok(answer);
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
