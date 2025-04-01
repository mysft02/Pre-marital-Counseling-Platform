using AutoMapper;
using SWP391.Domain;
using SWP391.Infrastructure.DataEnum;

namespace SWP391.DTO
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

    public class UserUpdateDTO
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string AvatarUrl { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserResponseDTO
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public UserRoleEnum Role { get; set; }
        public List<Booking> Bookings { get; set; }
    }
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserRegisterDTO>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ReverseMap();
        }
    }
}
