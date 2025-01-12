using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SWP391.Service;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly JwtService _jwtService;
        private readonly ILogger<PingController> _logger;

        public PingController(IConfiguration config, ILogger<PingController> logger, JwtService jwtService)
        {
            _config = config;
            _logger = logger;
            _jwtService = jwtService;
        }

        [HttpGet(Name = "GetPing")]
        public IActionResult Get()
        {
            _logger.LogInformation("Ping");
            var appVer = Environment.GetEnvironmentVariable("APP_VERSION");

            return Ok("Ok");
        }
    }
}
