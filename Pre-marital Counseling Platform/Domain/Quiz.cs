﻿using SWP391.Infrastructure.DataEnum;

namespace SWP391.Domain
{
    public class Quiz : BaseEntities
    {
        public Guid QuizId { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public QuizStatusEnum QuizStatus { get; set; }
        public Category Category { get; set; }
        public List<Question> Questions { get; set; } 
        public List<QuizResult> QuizResults { get; set; }
    }
}
