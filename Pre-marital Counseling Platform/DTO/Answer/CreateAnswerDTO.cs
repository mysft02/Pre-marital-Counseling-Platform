﻿namespace SWP391.DTO
{
    public class CreateAnswerDTO
    {
        public Guid QuestionId { get; set; }
        public string AnswerContent { get; set; }
        public decimal Score { get; set; }
    }
}
