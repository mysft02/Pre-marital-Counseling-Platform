using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;

namespace SWP391.DTO
{
    public class UserLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
