﻿namespace SWP391.DTO
{
    public class CreateMemberResultDTO
    {
        public Guid QuizId { get; set; }
        public Guid MemberId { get; set; }
        public Guid QuizResultId { get; set; }
        public decimal Score { get; set; }
    }
}
