using SWP391.Infrastructure.DataEnum;

namespace SWP391.DTO
{
    public class UserRegisterDTO
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public UserRoleEnum Role { get; set; }
    }
}
