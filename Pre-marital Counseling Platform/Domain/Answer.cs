namespace SWP391.Domain
{
    public class Answer : BaseEntities
    {
        public Guid AnswerId { get; set; }
        public Guid QuestionId { get; set; }
        public string AnswerContent { get; set; }
        public decimal Score { get; set; }
        public Question Question { get; set; }
    }
}
