namespace PostMVPProject.Models;


public class Repository
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string FullName { get; set; }
    public string HtmlUrl { get; set; }
    public string? Description { get; set; }
    public string? Language { get; set; }
    public int Stars { get; set; }
    public int Forks { get; set; }
    public int? OpenIssuesCount { get; set; }
    public bool HasIssues { get; set; }
    public bool HasProjects { get; set; }
    public List<string>? Topics { get; set; }
    public string? Visibility { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? PushedAt { get; set; }

    // Additional optional properties
    public string? CloneUrl { get; set; }
    public int? WatchersCount { get; set; }
    public string? License { get; set; }
    public string? ContributorsUrl { get; set; }
    public string? SubscribersUrl { get; set; }
    public string? CommitsUrl { get; set; }
    public string? GitCommitsUrl { get; set; }
    public string? IssuesUrl { get; set; }
    public string? PullsUrl { get; set; }
    public string? ReleasesUrl { get; set; }
    public string? TagsUrl { get; set; }

    // Owner Information
    public string? OwnerLogin { get; set; }
    public string? OwnerHtmlUrl { get; set; }

        // Navigation properties
    public ICollection<UserRepository> UserRepositories { get; set; }
    public ICollection<Issue> Issues { get; set; }  // Add missing relationship
    public ICollection<Annotation> Annotations { get; set; } // Fix name to plural

    public int? CategoryId { get; set; }
    public Category? Category { get; set; }




}
