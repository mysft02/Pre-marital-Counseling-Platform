using AutoMapper;
using FirebaseAdmin.Auth;
using Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SWP391.Domain;
using SWP391.DTO;
using SWP391.Infrastructure.DataEnum;
using SWP391.Infrastructure.DbContext;
using SWP391.Migrations;
using System.Data;
using System.Security.Claims;
using System.IO;

namespace SWP391.Service
{
    public interface IAuthService
    {
        Task<IActionResult> HandleLogin(UserLoginDTO userLoginDTO);
        Task<IActionResult> HandleRegister(UserRegisterDTO userRegisterDTO);
        Task<IActionResult> HandleUpdateProfile(UserUpdateDTO userUpdateDTO, string id);
    }

    public class AuthService : ControllerBase,IAuthService
    {
        private readonly PmcsDbContext _context;
        private readonly IConfiguration _config;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;

        public AuthService(PmcsDbContext context, IMapper mapper, IConfiguration config, JwtService jwtService, IHttpContextAccessor httpContextAccessor, IMemoryCache cache)
        {
            _context = context;
            _config = config;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<IActionResult> HandleUpdateProfile(UserUpdateDTO userUpdateDTO, string id)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(id));

                var userMapped = _mapper.Map<User>(userUpdateDTO);

                var avt = userUpdateDTO.AvatarUrl.Split(',')[1];
                userMapped.AvatarUrl = avt;

                _context.Update(userMapped);
                if(_context.SaveChanges() > 0)
                {
                    return Ok(user);
                }
                else
                {
                    return BadRequest("Update failed");
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleRegister(UserRegisterDTO userRegisterDTO)
        {
            try
            {
                var userDuplicate = _context.Users.FirstOrDefault(x => x.Email == userRegisterDTO.Email);

                if(userDuplicate != null) { return BadRequest("Email already exists"); }

                string imagePath = "Infrastructure/DefaultData/avatar.jpg"; // Thay đổi đường dẫn đến tệp ảnh của bạn

                // Chuyển đổi ảnh thành chuỗi Base64
                string base64String = _jwtService.ConvertImageToBase64(imagePath);


                var userMapped = _mapper.Map<User>(userRegisterDTO);
                userMapped.Password = BCrypt.Net.BCrypt.HashPassword(userRegisterDTO.Password);
                userMapped.IsActive = true;
                userMapped.AvatarUrl = base64String;

                _context.Add(userMapped);

                if(userMapped.Role == UserRoleEnum.THERAPIST)
                {
                    var createdTherapist = new TherapistCreateDTO
                    {
                        Status = true,
                        Avatar = imagePath,
                    };

                    var therapistMapped = _mapper.Map<Therapist>(createdTherapist);
                    therapistMapped.Description = "No Description";
                    therapistMapped.CreatedAt = DateTime.Now;
                    therapistMapped.UpdatedAt = DateTime.Now;
                    therapistMapped.CreatedBy = userMapped.UserId;
                    therapistMapped.UpdatedBy = userMapped.UserId;
                    therapistMapped.MeetUrl = "No Meet Url";
                    therapistMapped.Avatar = base64String;
                    therapistMapped.TherapistName = userMapped.FullName;

                    _context.Add(therapistMapped);
                }

                var wallet = new WalletCreateDTO
                {
                    UserId = userMapped.UserId,
                    Balance = 0,
                };

                var walletMapped = _mapper.Map<Wallet>(wallet);
                _context.Wallets.Add(walletMapped);

                await _context.SaveChangesAsync();

                // Trả về thông tin người dùng mới đã đăng ký
                return Ok(userMapped);
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
                    new Claim("UserId", loginUser.UserId.ToString()),
                    new Claim("Name", loginUser.FullName),
                    new Claim("Email", loginUser.Email),
                    new Claim("Phone", loginUser.Phone),
                    new Claim("Role", loginUser.Role.ToString()),
                    new Claim("Avatar", loginUser.AvatarUrl.ToString())
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
