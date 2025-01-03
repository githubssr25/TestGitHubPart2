using System.Net.Http.Headers;
using System.Text.Json;
using System.Collections.Generic; // Ensure this is included
using System.Net.Http.Headers; // Required for ProductInfoHeaderValue
using System.Text.Json; // Required for JSON serialization
using System.Text.Json.Serialization; // Handles case-insensitive deserialization
using TestDemo; // Single namespace for all models

var builder = WebApplication.CreateBuilder(args);

// Load configuration from appsettings.json
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var personalAccessToken = config["GitHub:PersonalAccessToken"];


//general URLs awesome for beginners: https://github.com/MunGell/awesome-for-beginners?tab=readme-ov-file#python

// up for grabs https://github.com/up-for-grabs/up-for-grabs.net/tree/gh-pages 

//good first issue dev wasnt workign btw but here https://goodfirstissue.dev/

// no public api for this https://www.codetriage.com/ 

//free code camp https://github.com/freecodecamp/freecodecamp/issues

//https://goodfirstissues.com/



// Helper method to create a reusable GitHub HttpClient
HttpClient CreateGitHubClient()
{
    var client = new HttpClient();
    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyApp", "1.0"));

    if (!string.IsNullOrWhiteSpace(personalAccessToken))
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", personalAccessToken);
    }

    return client;
}

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGet("/search", async (string query) =>
{
    var url = $"https://api.github.com/search/repositories?q={Uri.EscapeDataString(query)}";

    using var client = new HttpClient();
    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyApp", "1.0"));

    if (!string.IsNullOrWhiteSpace(personalAccessToken))
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", personalAccessToken);
    }

    var response = await client.GetAsync(url);

    if (!response.IsSuccessStatusCode)
    {
        return Results.StatusCode((int)response.StatusCode);
    }

    var jsonResponse = await response.Content.ReadAsStringAsync();

    try
    {
        // Parse and reshape the response
        var repositories = JsonSerializer.Deserialize<JsonElement>(jsonResponse)
            .GetProperty("items")
            .EnumerateArray()
            .Select(repo =>
            {
                try
                {
                    return new
                    {
                        Id = repo.TryGetProperty("id", out var id) ? id.GetInt32() : 0,
                        Name = repo.TryGetProperty("name", out var name) ? name.GetString() : "Unknown",
                        FullName = repo.TryGetProperty("full_name", out var fullName) ? fullName.GetString() : "Unknown",
                        Owner = repo.TryGetProperty("owner", out var owner)
                            ? new
                            {
                                Login = owner.TryGetProperty("login", out var login) ? login.GetString() : "Unknown",
                                HtmlUrl = owner.TryGetProperty("html_url", out var htmlUrl) ? htmlUrl.GetString() : "Unknown"
                            }
                            : null,
                        HtmlUrl = repo.TryGetProperty("html_url", out var repoHtmlUrl) ? repoHtmlUrl.GetString() : "Unknown",
                        CloneUrl = repo.TryGetProperty("clone_url", out var cloneUrl) ? cloneUrl.GetString() : "Unknown",
                        StargazersCount = repo.TryGetProperty("stargazers_count", out var stars) ? stars.GetInt32() : 0,
                        ForksCount = repo.TryGetProperty("forks_count", out var forks) ? forks.GetInt32() : 0,
                        WatchersCount = repo.TryGetProperty("watchers_count", out var watchers) ? watchers.GetInt32() : 0,
                        OpenIssuesCount = repo.TryGetProperty("open_issues_count", out var openIssues) ? openIssues.GetInt32() : 0,
                        Language = repo.TryGetProperty("language", out var language) ? language.GetString() : "Unknown",
                        Description = repo.TryGetProperty("description", out var description) ? description.GetString() : "No description",
                        PushedAt = repo.TryGetProperty("pushed_at", out var pushedAt) ? pushedAt.GetString() : "Not available",
                        HasIssues = repo.TryGetProperty("has_issues", out var hasIssues) ? hasIssues.GetBoolean() : false,
                        HasProjects = repo.TryGetProperty("has_projects", out var hasProjects) ? hasProjects.GetBoolean() : false,
                        Visibility = repo.TryGetProperty("visibility", out var visibility) ? visibility.GetString() : "public",
                        License = repo.TryGetProperty("license", out var license) && license.TryGetProperty("name", out var licenseName)
                            ? licenseName.GetString()
                            : "No license",
                        Topics = repo.TryGetProperty("topics", out var topics)
                            ? topics.EnumerateArray().Select(t => t.GetString()).ToList()
                            : new List<string>(),
                        ContributorsUrl = repo.TryGetProperty("contributors_url", out var contributorsUrl) ? contributorsUrl.GetString() : "Unknown",
                        SubscribersUrl = repo.TryGetProperty("subscribers_url", out var subscribersUrl) ? subscribersUrl.GetString() : "Unknown",
                        CommitsUrl = repo.TryGetProperty("commits_url", out var commitsUrl) ? commitsUrl.GetString() : "Unknown",
                        GitCommitsUrl = repo.TryGetProperty("git_commits_url", out var gitCommitsUrl) ? gitCommitsUrl.GetString() : "Unknown",
                        IssuesUrl = repo.TryGetProperty("issues_url", out var issuesUrl) ? issuesUrl.GetString() : "Unknown",
                        PullsUrl = repo.TryGetProperty("pulls_url", out var pullsUrl) ? pullsUrl.GetString() : "Unknown",
                        ReleasesUrl = repo.TryGetProperty("releases_url", out var releasesUrl) ? releasesUrl.GetString() : "Unknown",
                        TagsUrl = repo.TryGetProperty("tags_url", out var tagsUrl) ? tagsUrl.GetString() : "Unknown"
                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing repository: {ex.Message}");
                    return null; // Skip invalid repositories
                }
            })
            .Where(repo => repo != null) // Filter out invalid repositories
            .ToList();

        return Results.Json(repositories);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error parsing response: {ex.Message}");
        return Results.StatusCode(500);
    }
});




app.MapGet("/searchOpenIssueFlexibility", async (string query, int? minOpenIssues) =>
{
    var filter = query;

    // Add open issues filter dynamically if provided
    if (minOpenIssues.HasValue)
    {
        filter += $" open_issues:>{minOpenIssues.Value}";
    }

    var url = $"https://api.github.com/search/repositories?q={Uri.EscapeDataString(filter)}";

    using var client = new HttpClient();
    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyApp", "1.0"));

    var response = await client.GetAsync(url);
    if (!response.IsSuccessStatusCode) return Results.StatusCode((int)response.StatusCode);

    var jsonResponse = await response.Content.ReadAsStringAsync();
    return Results.Content(jsonResponse, "application/json");
});


//doesnt appear to be working 
// 1. Up-for-grabs endpoint: returns a list of projects as JSON
app.MapGet("/up-for-grabs", async (string? language) =>
{
    var url = "https://raw.githubusercontent.com/up-for-grabs/up-for-grabs.net/gh-pages/_data/projects.json";

    using var client = new HttpClient();
    var response = await client.GetAsync(url);
    if (!response.IsSuccessStatusCode) return Results.StatusCode((int)response.StatusCode);

    var jsonResponse = await response.Content.ReadAsStringAsync();
    var projects = JsonSerializer.Deserialize<List<UpForGrabsProject>>(jsonResponse) ?? new List<UpForGrabsProject>();

    if (!string.IsNullOrWhiteSpace(language))
    {
        projects = projects
            .Where(p => p.Tags != null && p.Tags.Any(t => t.Equals(language, StringComparison.OrdinalIgnoreCase)))
            .ToList();
    }

    return Results.Json(projects);
});


// 2. FreeCodeCamp issues: returns issues from their GitHub repo
app.MapGet("/fcc-issues", async (bool? goodFirstIssue = false, bool? helpWanted = false, string? state = "open") =>
{
    var url = "https://api.github.com/repos/freecodecamp/freecodecamp/issues";

    // Add optional query params for filtering
    var queryParams = new List<string>();

    if (goodFirstIssue == true)
    {
        queryParams.Add("labels=good+first+issue");
    }

    if (helpWanted == true)
    {
        queryParams.Add("labels=help+wanted");
    }

    if (!string.IsNullOrWhiteSpace(state))
    {
        queryParams.Add($"state={state}");
    }

    // Append the query params to the URL if any exist
    if (queryParams.Any())
    {
        url += "?" + string.Join("&", queryParams);
    }

    using var client = CreateGitHubClient();
    var response = await client.GetAsync(url);
    if (!response.IsSuccessStatusCode) return Results.StatusCode((int)response.StatusCode);

    var json = await response.Content.ReadAsStringAsync();
    return Results.Content(json, "application/json");
});




//works fine 
// 3. Awesome for Beginners: returns raw markdown of the README
app.MapGet("/awesome-beginners", async (
    string? query = null, 
    string? language = null, 
    string? name = null, 
    string? tag = null, 
    string? maintainer = null
) =>
{
    var url = "https://raw.githubusercontent.com/MunGell/awesome-for-beginners/master/README.md";
    using var client = new HttpClient();
    var response = await client.GetAsync(url);
    if (!response.IsSuccessStatusCode) return Results.StatusCode((int)response.StatusCode);

    var markdown = await response.Content.ReadAsStringAsync();
    var repos = MarkdownRepositoryExtractor.ExtractRepositoriesFromMarkdown(markdown);

    // Apply filters
    if (!string.IsNullOrWhiteSpace(language))
        repos = repos.Where(r => r.Language?.Contains(language, StringComparison.OrdinalIgnoreCase) == true).ToList();

    if (!string.IsNullOrWhiteSpace(query))
        repos = repos.Where(r => r.Name?.Contains(query, StringComparison.OrdinalIgnoreCase) == true).ToList();

    return Results.Json(repos);
});






// 4. Search issues with "good first issue" label across all GitHub repos
app.MapGet("/search-issues", async () =>
{
    // Searching issues labeled "good first issue"
    var url = "https://api.github.com/search/issues?q=is:open+is:issue+label:good-first-issue";
    using var client = CreateGitHubClient();
    var response = await client.GetAsync(url);
    if (!response.IsSuccessStatusCode) return Results.StatusCode((int)response.StatusCode);

    var json = await response.Content.ReadAsStringAsync();
    return Results.Content(json, "application/json");
});

// Filtered endpoint example
app.MapGet("/fcc-issues/filter", async (string? label, string? state, int? limit) =>
{
    var url = "https://api.github.com/repos/freeCodeCamp/freeCodeCamp/issues";
    using var client = CreateGitHubClient();
    var response = await client.GetAsync(url);
    if (!response.IsSuccessStatusCode) return Results.StatusCode((int)response.StatusCode);

    var json = await response.Content.ReadAsStringAsync();

    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    var issues = JsonSerializer.Deserialize<List<GitHubIssue>>(json, options) ?? new List<GitHubIssue>();

    if (!string.IsNullOrWhiteSpace(label))
    {
        issues = issues
            .Where(i => i.Labels.Any(l => l.Name.Equals(label, StringComparison.OrdinalIgnoreCase)))
            .ToList();
    }

    if (!string.IsNullOrWhiteSpace(state))
    {
        issues = issues
            .Where(i => i.State.Equals(state, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    if (limit.HasValue && limit.Value > 0)
    {
        issues = issues.Take(limit.Value).ToList();
    }

    return Results.Json(issues);
});

// Good first issue endpoint
app.MapGet("/fcc-issues/good-first", async () =>
{
    var url = "https://api.github.com/repos/freeCodeCamp/freeCodeCamp/issues";
    using var client = CreateGitHubClient();
    var response = await client.GetAsync(url);
    if (!response.IsSuccessStatusCode) return Results.StatusCode((int)response.StatusCode);

    var json = await response.Content.ReadAsStringAsync();

    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    var issues = JsonSerializer.Deserialize<List<GitHubIssue>>(json, options) ?? new List<GitHubIssue>();

    var filtered = issues.Where(i =>
        i.Labels.Any(l => l.Name.Equals("good first issue", StringComparison.OrdinalIgnoreCase))
    );

    return Results.Json(filtered);
});



app.MapGet("/github-contributions", async (string? language, string? label, int? limit) =>
{
    var query = "state:open"; // Base query to only show open issues

    // Add label filter
    if (!string.IsNullOrWhiteSpace(label))
    {
        query += $" label:\"{Uri.EscapeDataString(label)}\"";
    }
    else
    {
        query += " label:\"good first issue\"";
    }

    // Add language filter
    if (!string.IsNullOrWhiteSpace(language))
    {
        query += $" language:{Uri.EscapeDataString(language)}";
    }

    // URL to search GitHub issues
    var url = $"https://api.github.com/search/issues?q={query}&per_page={limit ?? 10}";

    using var client = new HttpClient();
    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyApp", "1.0"));
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "YOUR_PERSONAL_ACCESS_TOKEN");

    var response = await client.GetAsync(url);
    if (!response.IsSuccessStatusCode) return Results.StatusCode((int)response.StatusCode);

    var jsonResponse = await response.Content.ReadAsStringAsync();
    return Results.Content(jsonResponse, "application/json");
});



app.MapGet("/nss-search", async (
    string? query, 
    string? type = "repositories", 
    bool? goodFirstIssue = false, 
    bool? helpWanted = false, 
    int? minStars = null, 
    string? language = null,
    string? createdAfter = null, 
    string? updatedAfter = null
) =>
{
    // Default query to 'Nashville Software School' if no query is provided
    var searchQuery = string.IsNullOrWhiteSpace(query) ? "Nashville Software School" : query;

    // Build the filter query based on type
    string filter = $"{searchQuery}";

    // Add filters based on the user's request
    if (!string.IsNullOrWhiteSpace(language))
    {
        filter += $" language:{language}";
    }

    if (minStars.HasValue)
    {
        filter += $" stars:>{minStars.Value}";
    }

    if (!string.IsNullOrWhiteSpace(createdAfter))
    {
        filter += $" created:>{createdAfter}";
    }

    if (!string.IsNullOrWhiteSpace(updatedAfter))
    {
        filter += $" pushed:>{updatedAfter}";
    }

    if (type == "issues")
    {
        filter += " is:issue is:open";
        if (goodFirstIssue == true)
        {
            filter += " label:\"good first issue\"";
        }
        if (helpWanted == true)
        {
            filter += " label:\"help wanted\"";
        }
    }

    // API URL for GitHub search
    var url = $"https://api.github.com/search/{type}?q={Uri.EscapeDataString(filter)}";

    using var client = new HttpClient();
    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("NSSApp", "1.0"));

    var response = await client.GetAsync(url);
    if (!response.IsSuccessStatusCode) return Results.StatusCode((int)response.StatusCode);

    var jsonResponse = await response.Content.ReadAsStringAsync();
    return Results.Content(jsonResponse, "application/json");
});







app.Run();

