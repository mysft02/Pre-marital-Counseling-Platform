using AutoMapper;
using SWP391.Infrastructure.DataEnum;
using SWP391.Domain;

namespace SWP391.DTO
{
    public class BlogDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Body { get; set; }
        public BlogStatusEnum Status { get; set; }
        public string Picture { get; set; }
    }

    public class UpdateBlogDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Body { get; set; }
        public BlogStatusEnum Status { get; set; }
        public string Picture { get; set; }
    }

    public class CreateBlogDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Body { get; set; }
        public string Picture { get; set; }
    }

    public class BolgDTOProfife : Profile
    {
        public BolgDTOProfife()
        {
            CreateMap<Blog, BlogDTO>().ReverseMap();
            CreateMap<Blog, CreateBlogDTO>().ReverseMap();
            CreateMap<Blog, UpdateBlogDTO>().ReverseMap();
        }
    }
}
