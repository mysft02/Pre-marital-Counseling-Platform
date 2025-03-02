using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWP391.Infrastructure.DbContext;
using SWP391.Service;
using System.IO.Compression;
using System.Security.Claims;
using System.Text;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly JwtService _jwtService;
        private readonly ILogger<PingController> _logger;
        private readonly PmcsDbContext _dbContext;

        public PingController(IConfiguration config, ILogger<PingController> logger, JwtService jwtService, PmcsDbContext dbContext)
        {
            _config = config;
            _logger = logger;
            _jwtService = jwtService;
            _dbContext = dbContext;
        }

        [HttpGet(Name = "GetPing")]
        public IActionResult Get()
        {
            _logger.LogInformation("Ping");
            var appVer = Environment.GetEnvironmentVariable("APP_VERSION");

            return Ok("Ok");
        }

        [HttpGet("db", Name = "GetPingDb")]
        public IActionResult GetDb()
        {
            _logger.LogInformation("Ping DB");

            try
            {
                _dbContext.Database.OpenConnection();

                return Ok("Connect DB Success");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ping DB failed");

                return BadRequest("Connect DB Failed");
            }
        }

        [Authorize]
        [HttpGet("token", Name = "GetPingToken")]
        public IActionResult GetToken()
        {
            _logger.LogInformation("Ping Token");

            try
            {
                var currentUser = HttpContext.User;

                

                return Ok(currentUser.FindFirst(ClaimTypes.Email)?.Value);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Get token Failed");

                return BadRequest("Get token Failed");
            }
        }

        [HttpGet("Get_Base64_Zip", Name = "Get_Base64_Zip")]
        public IActionResult GetBase64Zip()
        {
            string imagePath = "wwwroot/image/avatar.jpg"; // Thay đổi đường dẫn đến tệp ảnh của bạn

            // Chuyển đổi ảnh thành chuỗi Base64
            string base64String = _jwtService.ConvertImageToBase64(imagePath);
            
            var result = _jwtService.CompressWithBrotli(imagePath);

            return Ok(result);
        }

        
    }
}
