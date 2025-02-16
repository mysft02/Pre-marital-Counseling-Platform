using SWP391.Infrastructure.DataEnum;

namespace SWP391.DTO.User
{
    public class UserDTO
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public UserRoleEnum Role { get; set; }
    }
}
