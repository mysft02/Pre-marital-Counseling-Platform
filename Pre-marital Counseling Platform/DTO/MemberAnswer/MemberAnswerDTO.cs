using SWP391.Domain;

namespace SWP391.DTO
{
    public class MemberAnswerDTO
    {
        public Guid MemberAnswerId { get; set; }
        public Guid MemberId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
    }

    public class MemberAnswerResponse
    {
        public QuizResult QuizResult { get; set; }
        public string UserScore { get; set; }
        public IEnumerable<Therapist> Therapists { get; set; }
    }
}
