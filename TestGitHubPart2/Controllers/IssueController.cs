using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Security.Claims;
using PostMVPProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using PostMVPProject.Models;




namespace postMVPFinalProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Ensures user is authenticated
    public class IssuesController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<User> _userManager;


        public IssuesController(
            IHttpClientFactory httpClientFactory,
            UserManager<User> userManager)
        {
            _httpClientFactory = httpClientFactory;
            _userManager = userManager;
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

                // Prepare the issue data
                var issueData = new
                {
                    title = issueDto.Title,
                    body = issueDto.Body,
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

                var createdIssue = await response.Content.ReadAsStringAsync();
                return Ok(JsonSerializer.Deserialize<JsonElement>(createdIssue));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}