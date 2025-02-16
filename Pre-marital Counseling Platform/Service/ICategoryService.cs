using Microsoft.AspNetCore.Mvc;
using SWP391.Domain;
using SWP391.DTO.Category;
using SWP391.DTO.Quiz;
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

        public CategoryService(PmcsDbContext context)
        {
            _context = context;
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
                var Category = new Category
                {
                    Name = categoryCreateDTO.Name,
                    Description = categoryCreateDTO.Description,
                    Status = CategoryStatusEnum.ACTIVE,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = Guid.Parse(userId),
                    UpdatedBy = Guid.Parse(userId)
                };

                var check = true;
                while (check)
                {
                    var id = Guid.NewGuid();
                    var checkId = _context.Categories.FirstOrDefault(x => x.CategoryId == id);
                    if (checkId == null)
                    {
                        Category.CategoryId = id;
                        check = false;
                    }
                }

                _context.Categories.Add(Category);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return Ok(Category);
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
