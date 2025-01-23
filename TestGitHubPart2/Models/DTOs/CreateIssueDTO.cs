
namespace PostMVPProject.Models.DTOs;

public class CreateIssueDTO
{
   public string Title { get; set; }
   public string Body { get; set; }
   public string RepositoryOwner { get; set; }
   public string RepositoryName { get; set; }
   public string RepositoryUrl { get; set; }
   public List<string> Labels { get; set; }
   public string? CodePath { get; set; }
   public string? CodeSnippet { get; set; }
   public string? StepsToReproduce { get; set; }
   public string? ExpectedBehavior { get; set; }
   public string? ActualBehavior { get; set; }
   public string? Environment { get; set; }
   public string? GitHubIssueId { get; set; }
   public string? State { get; set; } = "open";
   public string? HtmlUrl { get; set; }
   public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
   public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}