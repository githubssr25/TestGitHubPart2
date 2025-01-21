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


    // Navigation properties
    public int? RepositoryId { get; set; }
    public Repository? Repository { get; set; }
    public ICollection<UserIssue> UserIssues { get; set; }
}