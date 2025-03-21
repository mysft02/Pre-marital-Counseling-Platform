﻿using AutoMapper;
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
        Task<IActionResult> HandleLogout();
        Task<IActionResult> HandleGetWallet(string userId);
    }

    public class AuthService : ControllerBase, IAuthService
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

                user.FullName = userUpdateDTO.FullName;
                user.Phone = userUpdateDTO.Phone;
                user.IsActive = userUpdateDTO.IsActive;
                user.AvatarUrl = userUpdateDTO.AvatarUrl;

                _context.Users.Update(user);
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
                var userDuplicate = await _context.Users.FirstOrDefaultAsync(x => x.Email == userRegisterDTO.Email);

                if(userDuplicate != null) { return BadRequest("Email already exists"); }

                var userMapped = _mapper.Map<User>(userRegisterDTO);
                userMapped.Password = BCrypt.Net.BCrypt.HashPassword(userRegisterDTO.Password);
                userMapped.IsActive = true;
                userMapped.IsAdmin = false;
                userMapped.AvatarUrl = "https://firebasestorage.googleapis.com/v0/b/student-51e6a.appspot.com/o/images%2F638765994361515545_avatar.jpg?alt=media&token=9b3b9add-76dc-4729-84fa-e49b265def12";
                userMapped.CreatedAt = DateTime.Now;
                userMapped.UpdatedAt = DateTime.Now;
                userMapped.CreatedBy = userMapped.UserId;
                userMapped.UpdatedBy = userMapped.UserId;

                _context.Add(userMapped);

                if(userMapped.Role == UserRoleEnum.THERAPIST)
                {
                    var therapistMapped = new Therapist();
                    therapistMapped.TherapistId = userMapped.UserId;
                    therapistMapped.TherapistName = userMapped.FullName;
                    therapistMapped.Avatar = "https://firebasestorage.googleapis.com/v0/b/student-51e6a.appspot.com/o/images%2F638765994361515545_avatar.jpg?alt=media&token=9b3b9add-76dc-4729-84fa-e49b265def12";
                    therapistMapped.Status = true;
                    therapistMapped.Description = "No Description";
                    therapistMapped.ConsultationFee = 0;
                    therapistMapped.MeetUrl = "No Meet Url";
                    therapistMapped.CreatedAt = DateTime.Now;
                    therapistMapped.UpdatedAt = DateTime.Now;
                    therapistMapped.CreatedBy = userMapped.UserId;
                    therapistMapped.UpdatedBy = userMapped.UserId;

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

                var user = new UserLoginResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };

                // Trả về thông tin người dùng mới đã đăng ký
                return Ok(user);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleLogout()
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not logged in");
                }

                _cache.Remove($"RefreshToken_{userId}");

                _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Path = "/"
                });

                return Ok("Logged out successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> HandleGetWallet(string userId)
        {
            try
            {
                var userWallet = _context.Wallets
                    .FirstOrDefault(x => x.UserId == Guid.Parse(userId));

                var userTransaction = _context.Transactions
                    .Where(x => x.CreatedBy == Guid.Parse(userId))
                    .ToList();

                var response = new WalletResponseDTO
                {
                    Wallet = userWallet,
                    Transactions = userTransaction
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
