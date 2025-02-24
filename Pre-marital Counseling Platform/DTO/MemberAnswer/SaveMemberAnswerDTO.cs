namespace SWP391.DTO.MemberAnswer
{
    public class SaveMemberAnswerDTO
    {
        public Guid AnswerId { get; set; }
        public Guid MemberAnswerId { get; set; }
        public Guid MemberId { get; set; }
        public Guid QuestionId { get; set; }
    }
}
