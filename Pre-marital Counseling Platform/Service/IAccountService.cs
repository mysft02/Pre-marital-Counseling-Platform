using Microsoft.AspNetCore.Mvc;
using SWP391.Domain;
using SWP391.DTO.User;
using SWP391.Infrastructure.DbContext;

namespace SWP391.Service
{
    public interface IAccountService
    {
        Task<IActionResult> ChangePassword(ChangePasswordDTO changePassword, string? userId);
    }

    public class AccountService : ControllerBase, IAccountService
    {

        private readonly PmcsDbContext _context;

        public AccountService(PmcsDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePassword, string userId)
        {
            try
            {
                if (!Guid.TryParse(userId, out Guid userGuid))
                {
                    return BadRequest("Invalid user ID format.");
                }

                var user = await _context.Users.FindAsync(userGuid);
                if(user == null)
                {
                    return NotFound("User not found");
                }

                if(!BCrypt.Net.BCrypt.Verify(changePassword.OldPassword, user.Password))
                {
                    return BadRequest("Old password is not correct");
                }

                var hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(changePassword.NewPassword);

                user.Password = hashedNewPassword;
                await _context.SaveChangesAsync();

                return Ok("Password changed successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
