﻿namespace SWP391.DTO.MemberResult
{
    public class CalculateMemberResultDTO
    {
        public Guid MemberResultId { get; set; }
        public Guid QuizId { get; set; }
        public Guid MemberId { get; set; }
        public Guid QuizResultId { get; set; }
        public decimal Score { get; set; }
    }
}
