using Microsoft.AspNetCore.Mvc;
using SWP391.DTO;
using SWP391.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using SWP391.Domain;
using System;
using Microsoft.AspNetCore.Http.HttpResults;
using SWP391.DTO;
using AutoMapper;

namespace SWP391.Service
{
    public interface IMemberAnswerService
    {
        
        Task<IActionResult> GetAllMemberAnswers();
        Task<IActionResult> GetMemberAnswerById(Guid id);
        Task<IActionResult> CreateMemberAnswer(CreateMemberAnswerDTO createMemberAnswerDTO, string? userId);
        Task<IActionResult> UpdateMemberAnswer(UpdateMemberAnswerDTO updateMemberAnswerDTO, string? userId);
        Task<IActionResult> SaveMemberAnswer(List<CreateMemberAnswerDTO> dto, string? userId);

    }

    public class MemberAnswerService : ControllerBase, IMemberAnswerService
    {
        private readonly PmcsDbContext _context;
        private readonly IMapper _mapper;

        public MemberAnswerService(PmcsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> SaveMemberAnswer(List<CreateMemberAnswerDTO> dto, string? userId)
        {
            if (dto == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                decimal total = 0;
                foreach(var item in dto)
                {
                    var memberAnswerMappper = _mapper.Map<MemberAnswer>(item);
                    _context.MemberAnswers.Add(memberAnswerMappper);
                    var anwser = _context.Answers
                        .FirstOrDefault(x => x.AnswerId == item.AnswerId);
                    total += anwser.Score;
                }

                var quiz = _context.Questions
                    .Include(c => c.Quiz)
                    .FirstOrDefault(x => x.QuestionId == dto[0].QuestionId);
                var quizResult = await _context.QuizResults.FirstOrDefaultAsync(x => x.QuizId == quiz.QuizId && total <= x.Score);

                var memberResult = new CreateMemberResultDTO
                {
                    QuizId = quiz.QuizId,
                    MemberId = dto[0].MemberId,
                    Score = total,
                    QuizResultId = quizResult.QuizResultId
                };

                var memberResultMapper = _mapper.Map<MemberResult>(memberResult);
                _context.MemberResults.Add(memberResultMapper);

                var specification = _context.Specifications
                    .FirstOrDefault(x => x.Name == quizResult.Title && x.Level == quizResult.Level);

                var therapists = _context.TherapistSpecifications
                    .AsQueryable()
                    .Include(x => x.Therapist).ThenInclude(xc => xc.Schedules)
                    .Where(x => x.SpecificationId == specification.SpecificationId)
                    .GroupBy(x => x.SpecificationId)
                    .ToDictionary(g => g.Key, g => g.Select(x => x.Therapist));


                var response = new MemberAnswerResponse
                {
                    QuizResult = quizResult,
                    Therapists = therapists.TryGetValue(specification.SpecificationId, out var therapistsList) ? therapistsList : new List<Therapist>()
                };

                if (_context.SaveChanges() > 0)
                {
                    return Ok(response);
                }else
                {
                    return BadRequest("Failed to save");
                }


            }
            catch (Exception ex)
            {
                if(ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                }
                return BadRequest(ex.InnerException.Message);
            }
        }

        public async Task<IActionResult> GetAllMemberAnswers()
        {
            try
            {
                List<MemberAnswerDTO> memberAnswers = _context.MemberAnswers.Select(x => new MemberAnswerDTO
                {
                    MemberAnswerId = x.MemberAnswerId,
                    MemberId = x.MemberId,
                    QuestionId = x.QuestionId ?? Guid.Empty,
                    AnswerId = x.AnswerId,
                }).ToList();

                return Ok(memberAnswers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> GetMemberAnswerById(Guid id)
        {
            try
            {
                var memberAnswer = await _context.MemberAnswers
                    .Where(x => x.MemberAnswerId == id)
                    .Select(x => new MemberAnswerDTO
                    {
                        MemberAnswerId = x.MemberAnswerId,
                        MemberId = x.MemberId,
                        QuestionId = x.QuestionId ?? Guid.Empty,
                        AnswerId = x.AnswerId,
                    })
                    .FirstOrDefaultAsync();

                if (memberAnswer == null)
                {
                    return NotFound();
                }

                return Ok(memberAnswer);
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not find {ex.Message}");
            }
        }

        public async Task<IActionResult> CreateMemberAnswer(CreateMemberAnswerDTO createMemberAnswerDTO, string? userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User ID is required.");
                }

                var memberAnswer = new MemberAnswer
                {
                    MemberId = createMemberAnswerDTO.MemberId,
                    QuestionId = createMemberAnswerDTO.QuestionId,
                    AnswerId = createMemberAnswerDTO.AnswerId,
                    //CreatedAt = DateTime.Now,
                    //UpdatedAt = DateTime.Now,
                    //CreatedBy = Guid.Parse(userId),
                    //UpdatedBy = Guid.Parse(userId)
                };

                var check = true;
                while (check)
                {
                    var id = Guid.NewGuid();
                    var checkId = _context.MemberAnswers.FirstOrDefault(x => x.MemberAnswerId == id);
                    if (checkId == null)
                    {
                        memberAnswer.MemberAnswerId = id;
                        check = false;
                    }
                }

                _context.MemberAnswers.Add(memberAnswer);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return Ok(memberAnswer);
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

        public async Task<IActionResult> UpdateMemberAnswer(UpdateMemberAnswerDTO updateMemberAnswerDTO, string? userId)
        {
            try
            {
                var memberAnswer = _context.MemberAnswers.FirstOrDefault(x => x.AnswerId == updateMemberAnswerDTO.AnswerId);

                memberAnswer.MemberAnswerId = updateMemberAnswerDTO.MemberAnswerId;
                memberAnswer.AnswerId = updateMemberAnswerDTO.AnswerId;
                memberAnswer.QuestionId = updateMemberAnswerDTO.QuestionId;

                _context.MemberAnswers.Update(memberAnswer);
                if (_context.SaveChanges() > 0)
                {
                    return Ok(memberAnswer);
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
