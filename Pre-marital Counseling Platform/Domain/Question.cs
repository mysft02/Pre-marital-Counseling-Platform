namespace SWP391.Domain
{
    public class Question : BaseEntities
    {
        public Guid QuestionId { get; set; }
        public Guid QuizId { get; set; }
        public string QuestionContent { get; set; }
        public Quiz Quiz { get; set; }
    }
}
