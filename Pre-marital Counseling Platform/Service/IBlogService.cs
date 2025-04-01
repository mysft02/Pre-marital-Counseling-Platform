using AutoMapper;
using Ganss.Xss;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391.Domain;
using SWP391.DTO;
using SWP391.Infrastructure.DataEnum;
using SWP391.Infrastructure.DbContext;

namespace SWP391.Service
{
    public interface IBlogService
    {
        Task<IActionResult> GetAllBlog();
        Task<IActionResult> GetBlogById(Guid id);
        Task<IActionResult> CreateBlog(CreateBlogDTO dto, string? userId);
        Task<IActionResult> UpdateBlog(UpdateBlogDTO dto, string? userId);
        Task<IActionResult> DeleteBlog(Guid id, string? userId);
    }

    public class BlogServie : ControllerBase, IBlogService
    {

        private readonly PmcsDbContext _context;
        private readonly IMapper _mapper;
        private readonly HtmlSanitizer _sanitizer;

        public BlogServie(PmcsDbContext context, IMapper mapper, HtmlSanitizer sanitizer)
        {
            _context = context;
            _mapper = mapper;
            _sanitizer = sanitizer ?? new HtmlSanitizer();
        }

        public async Task<IActionResult> CreateBlog(CreateBlogDTO dto, string? userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid parsedUserId))
                {
                    return BadRequest("Invalid userId");
                }

                if (string.IsNullOrEmpty(dto.Body))
                {
                    return BadRequest("Body cannot be empty");
                }

                var nBlog = _mapper.Map<Blog>(dto);
                nBlog.Id = Guid.NewGuid();
                nBlog.Status = BlogStatusEnum.Active;
                nBlog.CreatedBy = Guid.Parse(userId);
                nBlog.CreatedAt = DateTime.Now;
                nBlog.UpdatedBy = Guid.Parse(userId);
                nBlog.UpdatedAt = DateTime.Now;

                var sanitizer = new HtmlSanitizer();
                sanitizer.AllowedTags.Clear();
                sanitizer.AllowedTags.Add("p");
                sanitizer.AllowedTags.Add("ul");
                sanitizer.AllowedTags.Add("ol");
                sanitizer.AllowedTags.Add("li");
                sanitizer.AllowedTags.Add("strong");
                sanitizer.AllowedTags.Add("em");
                sanitizer.AllowedTags.Add("img");
                sanitizer.AllowedTags.Add("br");
                sanitizer.AllowedAttributes.Add("src");
                sanitizer.AllowedAttributes.Add("alt");

                string processedBody = ConvertRawTextToHtml(dto.Body);
                nBlog.Body = sanitizer.Sanitize(processedBody);

                _context.Add(nBlog);
                var rs = await _context.SaveChangesAsync();
                if(rs > 0)
                {
                    return Ok("Create Successfully");
                }
                else
                {
                    return BadRequest("Create Fail");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string ConvertRawTextToHtml(string rawText)
        {
            if (string.IsNullOrEmpty(rawText))
                return rawText;

            var paragraphs = rawText.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => $"<p>{p.Trim().Replace("\r\n", "<br>").Replace("\n", "<br>")}</p>")
                .ToArray();

            return string.Join("", paragraphs);
        }

        public async Task<IActionResult> DeleteBlog(Guid id, string? userId)
        {
            try
            {
                var blog = _context.Blogs.FirstOrDefault(x => x.Id == id);
                if (blog == null)
                {
                    return NotFound("Blog not found.");
                }

                blog.Status = BlogStatusEnum.Inactive;

                _context.Blogs.Update(blog);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return Ok("delete successfully.");
                }
                else
                {
                    return BadRequest("Failed to delete.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> GetAllBlog()
        {
            try
            {
                var list = _context.Blogs.ToList();
                if(list.Count == 0)
                {
                    return NotFound("Blogs is empty");
                }
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> GetBlogById(Guid id)
        {
            try
            {
                var blog = _context.Blogs.Where(x => x.Status == BlogStatusEnum.Active && x.Id == id).FirstOrDefault();
                if(blog == null)
                {
                    return NotFound("Cannot find any Blog with this ID");
                }
                return Ok(blog);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> UpdateBlog(UpdateBlogDTO dto, string? userId)
        {
            try
            {
                var old = await _context.Blogs.AsNoTracking().FirstOrDefaultAsync(b => b.Id == dto.Id);

                var nBlog = _mapper.Map<Blog>(dto);
                nBlog.CreatedAt = old.CreatedAt;
                nBlog.CreatedBy = old.CreatedBy;
                nBlog.UpdatedBy = Guid.Parse(userId);
                nBlog.UpdatedAt = DateTime.Now;

                var sanitizer = new HtmlSanitizer();
                nBlog.Body = sanitizer.Sanitize(dto.Body);

                _context.Blogs.Update(nBlog);
                var rs = _context.SaveChanges();
                if (rs > 0)
                {
                    return Ok("Update Successfully");
                }
                else
                {
                    return BadRequest("Update Fail");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
