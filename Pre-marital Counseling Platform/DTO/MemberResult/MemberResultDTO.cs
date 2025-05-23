﻿using SWP391.Domain;

namespace SWP391.DTO
{
    public class MemberResultDTO
    {
        public Guid MemberResultId { get; set; }
        public Guid QuizId { get; set; }
        public Guid MemberId { get; set; }
        public Guid QuizResultId { get; set; }
        public decimal Score { get; set; }
        
    }

    public class MemberResultResponseDTO
    {
        public Guid MemberResultId { get; set; }
        public string QuizName { get; set; }
        public decimal Score { get; set; }
        public Quiz Quiz { get; set; }
    }
}
