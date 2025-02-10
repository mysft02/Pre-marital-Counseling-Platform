using SWP391.Infrastructure.DataEnum;

namespace SWP391.DTO.Question
{
    public class QuestionUpdateDTO
    {
        public Guid QuestionId { get; set; }
        public Guid QuizId { get; set; }
        public string QuestionContent { get; set; }
        public QuestionStatusEnum QuestionStatus { get; set; }
    }
}
