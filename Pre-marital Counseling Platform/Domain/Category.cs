namespace SWP391.Domain
{
    public class Category : BaseEntities
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
