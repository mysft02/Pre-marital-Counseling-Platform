namespace SWP391.Domain
{
    public class MemberResult
    {
        public Guid MemberResultId { get; set; }
        public Guid QuizId { get; set; }
        public Guid MemberId { get; set; }
        public Guid QuizResultId { get; set; }
        public decimal Score { get; set; }
        public Quiz Quiz { get; set; }
        public User User { get; set; }
        public QuizResult QuizResult { get; set; }
    }
}
