using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SWP391.Domain;
using SWP391.DTO;
using SWP391.Infrastructure.DataEnum;
using SWP391.Infrastructure.DbContext;

namespace SWP391.Service
{
    public interface ICategoryService
    {
        Task<IActionResult> HandleCreateCategory(CategoryCreateDTO categoryCreateDTO, string? userId);
        Task<IActionResult> HandleGetAllCategories();
        Task<IActionResult> HandleGetCategoryById(Guid id);
        Task<IActionResult> HandleUpdateCategory(CategoryUpdateDTO categoryUpdateDTO, string? userId);
    }

    public class CategoryService : ControllerBase, ICategoryService
    {
        private readonly PmcsDbContext _context;
        private IMapper _mapper;

        public CategoryService(PmcsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> HandleGetAllCategories()
        {
            try
            {
                List<CategoryDTO> categories = new List<CategoryDTO>();
                categories = _context.Categories
                    .Select(x => new CategoryDTO
                    {
                        CategoryId = x.CategoryId,
                        Name = x.Name,
                        Description = x.Description,
                        Status = x.Status
                    })
                    .ToList();

                return Ok(categories);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleGetCategoryById(Guid id)
        {
            try
            {
                var Category = _context.Categories
                    .Select(x => new CategoryDTO
                    {
                        CategoryId = x.CategoryId,
                        Name = x.Name,
                        Description = x.Description,
                        Status = x.Status
                    })
                    .Where(x => x.CategoryId == id);

                return Ok(Category);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleCreateCategory(CategoryCreateDTO categoryCreateDTO, string? userId)
        {
            try
            {
                var category = new CategoryDTO
                {
                    Name = categoryCreateDTO.Name,
                    Description = categoryCreateDTO.Description,
                    Status = CategoryStatusEnum.ACTIVE,
                };

                var categoryMapped = _mapper.Map<Category>(category);
                categoryMapped.CreatedAt = DateTime.Now;
                categoryMapped.UpdatedAt = DateTime.Now;
                categoryMapped.CreatedBy = Guid.Parse(userId);
                categoryMapped.UpdatedBy = Guid.Parse(userId);

                _context.Categories.Add(categoryMapped);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return Ok(categoryMapped);
                    return Ok(categoryMapped);
                }
                else
                {
                    return BadRequest("Create failed");
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleUpdateCategory(CategoryUpdateDTO categoryUpdateDTO, string? userId)
        {
            try
            {
                var category = _context.Categories.FirstOrDefault(x => x.CategoryId == categoryUpdateDTO.CategoryId);

                category.Name = categoryUpdateDTO.Name;
                category.Description = categoryUpdateDTO.Description;
                category.Status = (CategoryStatusEnum)categoryUpdateDTO.Status;
                category.UpdatedAt = DateTime.Now;
                category.UpdatedBy = Guid.Parse(userId);

                _context.Categories.Update(category);
                if (_context.SaveChanges() > 0)
                {
                    return Ok(category);
                }
                else
                {
                    return BadRequest("Update failed");
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
