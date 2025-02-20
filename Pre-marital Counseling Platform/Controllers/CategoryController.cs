using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391.DTO.Category;
using SWP391.DTO.Quiz;
using SWP391.Service;
using System.Security.Claims;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IConfiguration _config;

        public CategoryController(IConfiguration config, ICategoryService categoryService)
        {
            _config = config;
            _categoryService = categoryService;
        }

        [HttpGet("Get_All_Categories")]
        public async Task<IActionResult> GetAllCategories()
        {

            return await _categoryService.HandleGetAllCategories();
        }

        [HttpGet("Get_Category_By_Id")]
        public async Task<IActionResult> GetCategoryById([FromQuery] Guid id)
        {

            return await _categoryService.HandleGetCategoryById(id);
        }

        [Authorize]
        [HttpPost("Create_Category")]
        public async Task<IActionResult> CreateQuiz([FromBody] CategoryCreateDTO categoryCreateDTO)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst(ClaimTypes.Sid)?.Value;

            return await _categoryService.HandleCreateCategory(categoryCreateDTO, userId);
        }

        [Authorize]
        [HttpPost("Update_Category")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryUpdateDTO categoryUpdateDTO)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst(ClaimTypes.Sid)?.Value;

            return await _categoryService.HandleUpdateCategory(categoryUpdateDTO, userId);
        }
    }
}
