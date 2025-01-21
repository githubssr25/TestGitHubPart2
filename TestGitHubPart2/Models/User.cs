using Microsoft.AspNetCore.Identity;

namespace PostMVPProject.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    // Navigation properties
    public ICollection<UserRepository> UserRepositories { get; set; }
    public ICollection<Annotation> Annotations { get; set; }
    public ICollection<UserIssue> UserIssues { get; set; }  // Fix missing relationship
}


