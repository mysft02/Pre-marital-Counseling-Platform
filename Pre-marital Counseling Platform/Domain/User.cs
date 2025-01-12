namespace SWP391.Domain;

public class User
{
    public Guid UserId { get; set; }
    public string FullName { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string Email { get; internal set; }
}
