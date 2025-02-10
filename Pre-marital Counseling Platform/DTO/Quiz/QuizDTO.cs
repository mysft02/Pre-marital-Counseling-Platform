using SWP391.Infrastructure.DataEnum;

namespace SWP391.DTO.Quiz
{
    public class QuizDTO
    {
        public Guid QuizId { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public QuizStatusEnum Status { get; set; }
    }
}
