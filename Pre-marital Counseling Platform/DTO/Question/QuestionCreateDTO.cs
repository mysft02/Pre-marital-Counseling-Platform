namespace SWP391.DTO
{
    public class QuestionCreateDTO
    {
        public Guid QuizId { get; set; }
        public string QuestionContent { get; set; }
        public List<AnswerCreateDTO> Answers { get; set; }
    }

    public class AnswerCreateDTO
    {
        public string AnswerContent { get; set; }
        public decimal Score { get; set; }
    }
}
