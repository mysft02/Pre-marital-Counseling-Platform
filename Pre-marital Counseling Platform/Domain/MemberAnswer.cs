namespace SWP391.Domain
{
    public class MemberAnswer 
    {
        public Guid MemberAnswerId { get; set; }
        public Guid MemberId { get; set; }
        public Guid? QuestionId { get; set; }
        public Guid AnswerId { get; set; }
        public User Member { get; set; }
        public Question? Question { get; set; }
        public Answer Answer { get; set; }
    }
}
