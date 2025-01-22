using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Security.Claims;
using PostMVPProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using PostMVPProject.Models;


namespace PostMVPFinalProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepositoryController : ControllerBase
    {
         [HttpGet("search-repositories")]
        public async Task<IActionResult> SearchRepositories(
            string query,
            int? minStars = null,
            int? maxStars = null,
            string? language = null,
            string? createdAfter = null,
            string? updatedAfter = null,
            string? pushedBefore = null,
            bool? hasOpenIssues = null,
            string? topics = null,
            string? visibility = null,
            string? readmeKeyword = null)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query parameter is required.");
            }

            try
            {
                string filter = $"{query}";
                if (!string.IsNullOrWhiteSpace(language)) filter += $" language:{language}";
                if (minStars.HasValue) filter += $" stars:>{minStars.Value}";
                if (maxStars.HasValue) filter += $" stars:<{maxStars.Value}";
                if (!string.IsNullOrWhiteSpace(createdAfter)) filter += $" created:>{createdAfter}";
                if (!string.IsNullOrWhiteSpace(updatedAfter)) filter += $" pushed:>{updatedAfter}";
                if (!string.IsNullOrWhiteSpace(pushedBefore)) filter += $" pushed:<{pushedBefore}";
                if (hasOpenIssues == true) filter += " has:issues";
                if (!string.IsNullOrWhiteSpace(topics)) filter += $" topic:{topics}";
                if (!string.IsNullOrWhiteSpace(visibility)) filter += $" visibility:{visibility}";
                if (!string.IsNullOrWhiteSpace(readmeKeyword)) filter += $" in:readme {readmeKeyword}";

                var url = $"https://api.github.com/search/repositories?q={Uri.EscapeDataString(filter)}";

                using var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("App", "1.0"));

                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode);
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var repositories = JsonSerializer.Deserialize<JsonElement>(jsonResponse)
                    .GetProperty("items")
                    .EnumerateArray()
                    .Select(repo =>
                    {
                        if (repo.ValueKind != JsonValueKind.Object)
                        {
                            return null;
                        }

                        JsonElement ownerVal = default;
                        bool hasOwner = repo.TryGetProperty("owner", out ownerVal) && ownerVal.ValueKind == JsonValueKind.Object;

                        return new Repository
                        {
                            Id = repo.TryGetProperty("id", out var id) && id.ValueKind == JsonValueKind.Number ? id.GetInt32() : 0,
                            Name = repo.TryGetProperty("name", out var name) && name.ValueKind == JsonValueKind.String ? name.GetString() : "Unknown",
                            FullName = repo.TryGetProperty("full_name", out var fullName) && fullName.ValueKind == JsonValueKind.String ? fullName.GetString() : "Unknown",
                            HtmlUrl = repo.TryGetProperty("html_url", out var htmlUrl) && htmlUrl.ValueKind == JsonValueKind.String ? htmlUrl.GetString() : "Unknown",
                            Description = repo.TryGetProperty("description", out var description) && description.ValueKind == JsonValueKind.String ? description.GetString() : null,
                            Language = repo.TryGetProperty("language", out var language) && language.ValueKind == JsonValueKind.String ? language.GetString() : "Unknown",
                            Stars = repo.TryGetProperty("stargazers_count", out var stars) && stars.ValueKind == JsonValueKind.Number ? stars.GetInt32() : 0,
                            Forks = repo.TryGetProperty("forks_count", out var forks) && forks.ValueKind == JsonValueKind.Number ? forks.GetInt32() : 0,
                            WatchersCount = repo.TryGetProperty("watchers_count", out var watchers) && watchers.ValueKind == JsonValueKind.Number ? watchers.GetInt32() : 0,
                            Visibility = repo.TryGetProperty("visibility", out var visibility) && visibility.ValueKind == JsonValueKind.String ? visibility.GetString() : "public",
                            License = repo.TryGetProperty("license", out var license) && license.ValueKind == JsonValueKind.Object &&
                                    license.TryGetProperty("name", out var licenseName) && licenseName.ValueKind == JsonValueKind.String
                                    ? licenseName.GetString() : null,
                            CreatedAt = repo.TryGetProperty("created_at", out var createdAt) && createdAt.ValueKind == JsonValueKind.String &&
                                      DateTime.TryParse(createdAt.GetString(), out var createdDate) ? createdDate : (DateTime?)null,
                            UpdatedAt = repo.TryGetProperty("updated_at", out var updatedAt) && updatedAt.ValueKind == JsonValueKind.String &&
                                      DateTime.TryParse(updatedAt.GetString(), out var updatedDate) ? updatedDate : (DateTime?)null,
                            PushedAt = repo.TryGetProperty("pushed_at", out var pushedAt) && pushedAt.ValueKind == JsonValueKind.String &&
                                     DateTime.TryParse(pushedAt.GetString(), out var pushedDate) ? pushedDate : (DateTime?)null,
                            CloneUrl = repo.TryGetProperty("clone_url", out var cloneUrl) && cloneUrl.ValueKind == JsonValueKind.String ? cloneUrl.GetString() : null,
                            IssuesUrl = repo.TryGetProperty("issues_url", out var issuesUrl) && issuesUrl.ValueKind == JsonValueKind.String ? issuesUrl.GetString() : null,
                            PullsUrl = repo.TryGetProperty("pulls_url", out var pullsUrl) && pullsUrl.ValueKind == JsonValueKind.String ? pullsUrl.GetString() : null,
                            ReleasesUrl = repo.TryGetProperty("releases_url", out var releasesUrl) && releasesUrl.ValueKind == JsonValueKind.String ? releasesUrl.GetString() : null,
                            OwnerLogin = hasOwner && ownerVal.TryGetProperty("login", out var login) && login.ValueKind == JsonValueKind.String ? login.GetString() : null,
                            OwnerHtmlUrl = hasOwner && ownerVal.TryGetProperty("html_url", out var ownerHtmlUrl) && ownerHtmlUrl.ValueKind == JsonValueKind.String ? ownerHtmlUrl.GetString() : null
                        };
                    })
                    .Where(repo => repo != null)
                    .ToList();

                return Ok(repositories);
            }
            catch (Exception ex)
            {
                return Problem($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}