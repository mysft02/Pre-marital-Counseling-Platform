namespace SWP391.Domain
{
    public class BaseEntities
    {
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public User CreatedUser { get; set; } = null!;
        public User UpdatedUser { get; set; } = null!;
    }
}
