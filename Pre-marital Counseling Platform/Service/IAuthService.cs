using Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SWP391.Domain;
using SWP391.DTO.User;
using SWP391.Infrastructure.DbContext;
using SWP391.Migrations;
using System.Data;
using System.Security.Claims;

namespace SWP391.Service
{
    public interface IAuthService
    {
        Task<IActionResult> HandleLogin(UserLoginDTO userLoginDTO);
        Task<IActionResult> HandleRegister(UserRegisterDTO userRegisterDTO);
    }

    public class AuthService : ControllerBase,IAuthService
    {
        private readonly PmcsDbContext _context;
        private readonly IConfiguration _config;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _cache;

        public AuthService(PmcsDbContext context, IConfiguration config, JwtService jwtService, IHttpContextAccessor httpContextAccessor, IMemoryCache cache)
        {
            _context = context;
            _config = config;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
        }

        public async Task<IActionResult> HandleRegister(UserRegisterDTO userRegisterDTO)
        {
            try
            {
                var createdUser = new User();

                var userDuplicate = _context.Users.FirstOrDefault(x => x.Email == userRegisterDTO.Email);

                if(userDuplicate != null) { return BadRequest("Email already exists"); }

                createdUser.FullName = userRegisterDTO.FullName;
                createdUser.Phone = userRegisterDTO.Phone;
                createdUser.Email = userRegisterDTO.Email;
                createdUser.Password = BCrypt.Net.BCrypt.HashPassword(userRegisterDTO.Password);
                createdUser.Role = userRegisterDTO.Role;

                var check = true;

                while (check)
                {
                    var id = Guid.NewGuid();
                    var checkId = _context.Users.FirstOrDefault(x => x.UserId == id);
                    if (checkId == null)
                    {
                        createdUser.UserId = id;
                        check = false;
                    }
                }

                createdUser.CreatedAt = DateTime.Now;
                createdUser.UpdatedAt = DateTime.Now;
                createdUser.CreatedBy = createdUser.UserId;
                createdUser.UpdatedBy = createdUser.UserId;

                _context.Add(createdUser);
                await _context.SaveChangesAsync();

                // Trả về thông tin người dùng mới đã đăng ký
                return Ok(createdUser);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleLogin(UserLoginDTO userLoginDTO)
        {
            try
            {
                var loginUser = _context.Users.FirstOrDefault(x => x.Email == userLoginDTO.Email);

                if (loginUser == null || !BCrypt.Net.BCrypt.Verify(userLoginDTO.Password, loginUser.Password))
                {
                    return Unauthorized("Invalid username or password");
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, loginUser.UserId.ToString()),
                    new Claim(ClaimTypes.Email, loginUser.Email),
                    new Claim(ClaimTypes.Role, loginUser.Role.ToString())
                };

                // Tạo access token
                var accessToken = _jwtService.GenerateAccessToken(claims);

                // Tạo refresh token
                var refreshToken = _jwtService.GenerateRefreshToken();

                // Lưu refresh token vào cache
                _cache.Set($"RefreshToken_{loginUser.UserId}", refreshToken, TimeSpan.FromDays(7));

                // Đặt refresh token vào cookie
                _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });

                var user = new UserLoginResponse{
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };

                // Trả về thông tin người dùng mới đã đăng ký
                return Ok(user);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
