﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391.Domain;
using SWP391.DTO;
using SWP391.Infrastructure.DbContext;

namespace SWP391.Service
{
    public interface IMemberResultService
    {
        Task<IActionResult> GetAllMemberResult();
        Task<IActionResult> GetMemberResultByUserId(Guid id);
        Task<IActionResult> CreateMemberResult(CreateMemberResultDTO dto, string? userId);
        Task<IActionResult> UpdateMemberResult(UpdateMemberResultDTO dto, string? userId);
        Task<IActionResult> CalculateMemberResult(CreateMemberResultDTO dto, string? userId);
    }

    public class MemberResultService : ControllerBase, IMemberResultService
    {
        private readonly PmcsDbContext _context;
        private readonly IMapper _mapper;

        public MemberResultService(PmcsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> GetAllMemberResult()
        {
            try
            {
                List<MemberResultDTO> memberResult = _context.MemberResults.Select(x => new MemberResultDTO
                {
                    MemberResultId = x.MemberResultId,
                    QuizId = x.QuizId,
                    MemberId = x.MemberId,
                    QuizResultId = x.QuizResultId,
                    Score = x.Score
                }).ToList();

                return Ok(memberResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> GetMemberResultByUserId(Guid id)
        {
            try
            {
                var memberResult = _context.MemberResults
                    .Include(x => x.Quiz)
                    .Where(x => x.MemberId == id)
                    .Select(x => new MemberResultResponseDTO
                {
                    MemberResultId = x.MemberResultId,
                    QuizName = x.Quiz.Name,
                    Score = x.Score,
                    Quiz = x.Quiz,
                })
                    .ToList();
                return Ok(memberResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        public async Task<IActionResult> CreateMemberResult(CreateMemberResultDTO dto, string? userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User ID is required.");
                }

                var memberResult = new MemberResult
                {
                    QuizId = dto.QuizId,
                    MemberId = dto.MemberId,
                    Score = dto.Score,
                    QuizResultId = dto.QuizResultId
                };

                var check = true;
                while (check)
                {
                    var id = Guid.NewGuid();
                    var checkId = _context.MemberResults.FirstOrDefault(x => x.MemberResultId == id);
                    if (checkId == null)
                    {
                        memberResult.MemberResultId = id;
                        check = false;
                    }
                }

                _context.MemberResults.Add(memberResult);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return Ok(memberResult);
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

        public async Task<IActionResult> UpdateMemberResult(UpdateMemberResultDTO dto, string? userId)
        {
            try
            {
                var memberResult = _context.MemberResults.FirstOrDefault(x => x.MemberResultId == dto.MemberResultId);

                memberResult.MemberResultId = dto.MemberResultId;
                memberResult.QuizId = dto.QuizId;
                memberResult.MemberId = dto.MemberId;
                memberResult.QuizResultId = dto.QuizResultId;
                memberResult.Score = dto.Score;

                _context.MemberResults.Update(memberResult);
                if (_context.SaveChanges() > 0)
                {
                    return Ok(memberResult);
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

        public async Task<IActionResult> CalculateMemberResult(CreateMemberResultDTO dto, string? userId)
        {
            if (dto == null)
            {
                return BadRequest("Invalid data.");
            }
            try
            {

                var memberAnswers = _context.
                    MemberAnswers.
                    Where(x => x.MemberId == dto.MemberId && x.Question.QuizId == dto.QuizId).ToList();

                decimal totalScore = 0;

                foreach (var memberAnswer in memberAnswers)
                {
                    var answer = await _context.Answers.FirstOrDefaultAsync(x => x.AnswerId == memberAnswer.AnswerId);
                    if (answer != null)
                    {
                        totalScore += answer.Score;
                    }
                }
                
                var memberResultMapper = _mapper.Map<MemberResult>(dto);
                memberResultMapper.Score = totalScore;
                _context.MemberResults.Add(memberResultMapper);
                
                if (_context.SaveChanges() > 0)
                {
                    return Ok(memberResultMapper);
                }
                else
                {
                    return BadRequest("Failed to save");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
