﻿using SWP391.Domain;

namespace SWP391.DTO
{
    public class MemberAnswerDTO
    {
        public Guid MemberAnswerId { get; set; }
        public Guid MemberId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
    }
}
