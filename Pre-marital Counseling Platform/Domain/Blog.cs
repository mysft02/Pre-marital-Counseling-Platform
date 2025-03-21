using SWP391.Infrastructure.DataEnum;

namespace SWP391.Domain
{
    public class Blog : BaseEntities
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Body { get; set; }
        public BlogStatusEnum Status { get; set; }
        public string Picture { get; set; }
    }
}
