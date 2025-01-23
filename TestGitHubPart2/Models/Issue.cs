namespace PostMVPProject.Models;
public class Issue
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Body { get; set; }
    public string State { get; set; }
    public int Comments { get; set; }
    public string HtmlUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? RepositoryUrl { get; set; }
    public List<string>? Labels { get; set; }

      public string? GitHubIssueId { get; set; } // Add this

      // New optional fields
    public string? CodePath { get; set; }
    public string? CodeSnippet { get; set; }
    public string? StepsToReproduce { get; set; }
    public string? ExpectedBehavior { get; set; }
    public string? ActualBehavior { get; set; }
    public string? Environment { get; set; }


    // Navigation properties
    public int? RepositoryId { get; set; }
    public Repository? Repository { get; set; }
    public ICollection<UserIssue> UserIssues { get; set; }
}