using AutoMapper;
using SWP391.Domain;
using SWP391.Infrastructure.DataEnum;

namespace SWP391.DTO
{
    public class CategoryDTO
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CategoryStatusEnum Status { get; set; }
    }

    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryCreateDTO, Category>().ReverseMap();
        }
    }
}
