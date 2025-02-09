namespace SWP391.Domain
{
    public class Quiz : BaseEntities
    {
        public Guid QuizId { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
    }
}
