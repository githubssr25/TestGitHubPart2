using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Security.Claims;
using PostMVPProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using PostMVPProject.Models;
using PostMVPFinalProject.Context;




namespace postMVPFinalProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Ensures user is authenticated

public class IssuesController : ControllerBase
{
   private readonly IHttpClientFactory _httpClientFactory;
   private readonly UserManager<User> _userManager;
   private readonly GitHubPostMVPDbContext _context;

   public IssuesController(
       IHttpClientFactory httpClientFactory,
       UserManager<User> userManager,
       GitHubPostMVPDbContext context)
   {
       _httpClientFactory = httpClientFactory;
       _userManager = userManager;
       _context = context;
   }

   private string FormatIssueBody(CreateIssueDTO issueDto)
   {
       return $@"
           ### Description
           {issueDto.Body}

           {(string.IsNullOrEmpty(issueDto.CodePath) ? "" : $@"### Code Location
           `{issueDto.CodePath}`")}

           {(string.IsNullOrEmpty(issueDto.CodeSnippet) ? "" : $@"### Code Snippet
           `{issueDto.CodeSnippet}`")}

           {(string.IsNullOrEmpty(issueDto.StepsToReproduce) ? "" : $@"### Steps to Reproduce
           {issueDto.StepsToReproduce}")}

           {(string.IsNullOrEmpty(issueDto.ExpectedBehavior) ? "" : $@"### Expected Behavior
           {issueDto.ExpectedBehavior}")}

           {(string.IsNullOrEmpty(issueDto.ActualBehavior) ? "" : $@"### Actual Behavior
           {issueDto.ActualBehavior}")}

           {(string.IsNullOrEmpty(issueDto.Environment) ? "" : $@"### Environment
           {issueDto.Environment}")}"
       .Trim();
   }

   [HttpPost("create")]
   public async Task<IActionResult> CreateIssue([FromBody] CreateIssueDTO issueDto)
   {
       try
       {
           // Get current user
           var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
           var user = await _userManager.FindByIdAsync(userId);
           
           if (user == null || string.IsNullOrEmpty(user.GitHubToken))
           {
               return Unauthorized("GitHub authentication required");
           }

           // Create GitHub API client
           var client = _httpClientFactory.CreateClient();
           client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
           client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.GitHubToken);
           client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("YourApp", "1.0"));

           var formattedBody = FormatIssueBody(issueDto);

           // Prepare the issue data
           var issueData = new
           {
               title = issueDto.Title,
               body = formattedBody,
               labels = issueDto.Labels ?? new List<string>()
           };

           var content = new StringContent(
               JsonSerializer.Serialize(issueData),
               System.Text.Encoding.UTF8,
               "application/json");

           // Make request to GitHub API
           var response = await client.PostAsync(
               $"https://api.github.com/repos/{issueDto.RepositoryOwner}/{issueDto.RepositoryName}/issues",
               content);

           if (!response.IsSuccessStatusCode)
           {
               var errorContent = await response.Content.ReadAsStringAsync();
               return StatusCode((int)response.StatusCode,
                   $"GitHub API error: {response.StatusCode} - {errorContent}");
           }

           var gitHubResponse = await response.Content.ReadAsStringAsync();
           var gitHubIssue = JsonSerializer.Deserialize<JsonElement>(gitHubResponse);

           // Save to local DB
           var issue = new Issue
           {
               Title = issueDto.Title,
               Body = formattedBody,
               State = "open",
               RepositoryUrl = issueDto.RepositoryUrl,
               HtmlUrl = gitHubIssue.GetProperty("html_url").GetString(),
               GitHubIssueId = gitHubIssue.GetProperty("number").GetString(),
               CreatedAt = DateTime.UtcNow,
               UpdatedAt = DateTime.UtcNow,
               Labels = issueDto.Labels,
               CodePath = issueDto.CodePath,
               CodeSnippet = issueDto.CodeSnippet,
               StepsToReproduce = issueDto.StepsToReproduce,
               ExpectedBehavior = issueDto.ExpectedBehavior,
               ActualBehavior = issueDto.ActualBehavior,
               Environment = issueDto.Environment
           };

           _context.Issues.Add(issue);
           await _context.SaveChangesAsync();

           return Ok(new { gitHubIssue, localIssue = issue });
       }
       catch (Exception ex)
       {
           return StatusCode(500, $"An error occurred: {ex.Message}");
       }
   }

        [HttpGet("search-issues")]
        public async Task<IActionResult> SearchIssues(
            string query,
            bool? goodFirstIssue = false,
            bool? helpWanted = false,
            string? createdAfter = null,
            string? updatedAfter = null,
            int? minOpenIssues = null)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query parameter is required.");
            }

            try
            {
                string filter = $"{query} is:issue is:open";
                if (goodFirstIssue == true) filter += " label:\"good first issue\"";
                if (helpWanted == true) filter += " label:\"help wanted\"";
                if (minOpenIssues.HasValue) filter += $" open_issues:>{minOpenIssues.Value}";
                if (!string.IsNullOrWhiteSpace(createdAfter)) filter += $" created:>{createdAfter}";
                if (!string.IsNullOrWhiteSpace(updatedAfter)) filter += $" updated:>{updatedAfter}";

                var url = $"https://api.github.com/search/issues?q={Uri.EscapeDataString(filter)}";

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("App", "1.0"));

                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode);
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var rootElement = JsonSerializer.Deserialize<JsonElement>(jsonResponse);
                var items = rootElement.GetProperty("items");

                var issues = items.EnumerateArray().Select(issue => new
                {
                    Id = issue.TryGetProperty("id", out var id) ? id.GetInt64() : 0,
                    Title = issue.TryGetProperty("title", out var title) ? title.GetString() : "",
                    HtmlUrl = issue.TryGetProperty("html_url", out var htmlUrl) ? htmlUrl.GetString() : "",
                    State = issue.TryGetProperty("state", out var state) ? state.GetString() : "unknown",
                    Body = issue.TryGetProperty("body", out var body) ? body.GetString() : null,
                    CreatedAt = issue.TryGetProperty("created_at", out var createdAt) &&
                               createdAt.ValueKind == JsonValueKind.String &&
                               DateTime.TryParse(createdAt.GetString(), out var createdDate)
                               ? createdDate : (DateTime?)null,
                    UpdatedAt = issue.TryGetProperty("updated_at", out var updatedAt) &&
                               updatedAt.ValueKind == JsonValueKind.String &&
                               DateTime.TryParse(updatedAt.GetString(), out var updatedDate)
                               ? updatedDate : (DateTime?)null,
                    Comments = issue.TryGetProperty("comments", out var comments) ? comments.GetInt32() : 0,
                    RepositoryUrl = issue.TryGetProperty("repository_url", out var repoUrl) ? repoUrl.GetString() : null,
                    Labels = issue.TryGetProperty("labels", out var labels) && labels.ValueKind == JsonValueKind.Array
                        ? labels.EnumerateArray()
                            .Select(label => label.TryGetProperty("name", out var name) ? name.GetString() : "")
                            .Where(name => !string.IsNullOrEmpty(name))
                            .ToList()
                        : new List<string>()
                }).ToList();

                return Ok(new
                {
                    TotalCount = rootElement.GetProperty("total_count").GetInt32(),
                    Items = issues
                });
            }
            catch (Exception ex)
            {
                return Problem($"An error occurred: {ex.Message}");
            }
        }





        
    }
}