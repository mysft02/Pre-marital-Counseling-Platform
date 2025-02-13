namespace SWP391.Domain
{
    public class QuizResult : BaseEntities
    {
        public Guid QuizResultId { get; set; }
        public Guid QuizId { get; set; }
        public decimal Score { get; set; }
        public int Level { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Quiz Quiz { get; set; }
    }
}
