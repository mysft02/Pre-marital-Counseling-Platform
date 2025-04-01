using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391.DTO;
using SWP391.Service;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _service;

        public BlogController(IBlogService service)
        {
            _service = service;
        }

        [HttpGet("Get_All_Blog")]
        public async Task<IActionResult> GetAllBlog()
        {
            return await _service.GetAllBlog();
        }

        [HttpGet("Get_Blog_By_Id")]
        public async Task<IActionResult> GetBlogById(Guid id)
        {
            return await _service.GetBlogById(id);
        }

        //[Authorize]
        [HttpPost("Create_Blog")]
        public async Task<IActionResult> CreateBlog([FromForm] CreateBlogDTO dto)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;
            return await _service.CreateBlog(dto, userId);
        }

        [Authorize]
        [HttpPost("Update_Blog")]
        public async Task<IActionResult> UpdateBlog([FromBody] UpdateBlogDTO dto)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;
            return await _service.UpdateBlog(dto, userId);
        }

        [Authorize]
        [HttpPost("Delete_Blog")]
        public async Task<IActionResult> DeleteBlog(Guid id)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;
            return await _service.DeleteBlog(id, userId);
        }
    }
}
