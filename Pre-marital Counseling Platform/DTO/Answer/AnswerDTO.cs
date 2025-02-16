namespace SWP391.DTO.Answer
{
    public class AnswerDTO
    {
        public Guid AnswerId { get; set; }
        public Guid QuestionId { get; set; }
        public string? AnswerContent { get; set; }
        public decimal Score { get; set; }
    }
}
