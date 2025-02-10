using SWP391.Infrastructure.DataEnum;

namespace SWP391.DTO.Category
{
    public class CategoryUpdateDTO
    {
        public Guid? CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public CategoryStatusEnum? Status { get; set; }
    }
}
