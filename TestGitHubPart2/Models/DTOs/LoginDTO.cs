
namespace PostMVPProject.Models.DTOs;
public class LoginDTO
{
    public string Email { get; set; }
    public string Password { get; set; }

    public string? GitHubCode { get; set; }  // Optional GitHub authorization code
}
