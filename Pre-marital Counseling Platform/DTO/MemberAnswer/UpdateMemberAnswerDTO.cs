using SWP391.Domain;

namespace SWP391.DTO
{
    public class UpdateMemberAnswerDTO
    {
        public Guid MemberAnswerId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
    }
}
