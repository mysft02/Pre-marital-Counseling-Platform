using SWP391.Infrastructure.DataEnum;

namespace SWP391.Domain
{
    public class Category : BaseEntities
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CategoryStatusEnum Status { get; set; }
        public List<Quiz> Quizzes { get; set; }
    }
}
