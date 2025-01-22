using System.Net.Http.Headers;
using System.Text.Json;
using System.Collections.Generic; // Ensure this is included
using System.Text.Json.Serialization; // Handles case-insensitive deserialization
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using PostMVPFinalProject.Context;
using  PostMVPProject.Extractor;
using  PostMVPProject.Models;

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


// Add controllers
// builder.Services.AddControllers();

// Add controllers with JSON cycle handling
builder.Services.AddControllers().AddJsonOptions(opts => {
    opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Configure database context
builder.Services.AddDbContext<GitHubPostMVPDbContext>(options =>
    options.UseNpgsql(config.GetConnectionString("GitHubPostMVPConnection")));

    // Configure Identity
builder.Services.AddIdentityCore<User>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<GitHubPostMVPDbContext>()
.AddDefaultTokenProviders();

// Add HttpClient factory
builder.Services.AddHttpClient();

 // Add session services ADDING THIS 1-13-24 FOR OAUTH SECURITY 
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".GitHubAuth.Session";
    options.Cookie.HttpOnly = true; // Prevent JavaScript access to session cookie
    options.Cookie.IsEssential = true; // Ensure session works even if GDPR cookie consent is required
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Set session timeout
});


// Configure Identity (if needed)
// builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//     .AddEntityFrameworkStores<GitHubPostMVPDbContext>()
//     .AddDefaultTokenProviders();

// Configure Authentication
builder.Services.AddAuthentication(options => 
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.LoginPath = "/api/auth/login";
    options.LogoutPath = "/api/auth/logout";
    options.AccessDeniedPath = "/api/auth/denied";
    options.ExpireTimeSpan = TimeSpan.FromHours(24);
    options.SlidingExpiration = true;
});

// Add this line here!
builder.Services.AddAuthorization();

// Configure logging (optional)
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // Frontend URL
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); // Added for cookie auth
        });
});

var app = builder.Build();


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


// Use CORS policy
app.UseCors("AllowFrontend");

// Add authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllers();


//doesnt appear to be working 
// 1. Up-for-grabs endpoint: returns a list of projects as JSON
app.MapGet("/up-for-grabs", async (string? language) =>
{
    var url = "https://raw.githubusercontent.com/up-for-grabs/up-for-grabs.net/gh-pages/_data/projects.json";

    using var client = new HttpClient();
    var response = await client.GetAsync(url);
    if (!response.IsSuccessStatusCode) return Results.StatusCode((int)response.StatusCode);

    var jsonResponse = await response.Content.ReadAsStringAsync();
    var projects = JsonSerializer.Deserialize<List<JsonElement>>(jsonResponse) ?? new List<JsonElement>();

    if (!string.IsNullOrWhiteSpace(language))
    {
        projects = projects
            .Where(p => p.GetProperty("tags").EnumerateArray()
                .Any(tag => tag.GetString().Equals(language, StringComparison.OrdinalIgnoreCase)))
            .ToList();
    }

    return Results.Json(projects);
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



// Filtered endpoint example
app.MapGet("/fcc-issues/filter", async (string? label, string? state, int? limit) =>
{
    var url = "https://api.github.com/repos/freeCodeCamp/freeCodeCamp/issues";
    using var client = CreateGitHubClient();
    var response = await client.GetAsync(url);
    if (!response.IsSuccessStatusCode) return Results.StatusCode((int)response.StatusCode);

    var json = await response.Content.ReadAsStringAsync();

    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
      var issues = JsonSerializer.Deserialize<List<JsonElement>>(json, options) ?? new List<JsonElement>();

    if (!string.IsNullOrWhiteSpace(label))
    {
        issues = issues
            .Where(i => i.GetProperty("labels").EnumerateArray()
                .Any(l => l.GetProperty("name").GetString().Equals(label, StringComparison.OrdinalIgnoreCase)))
            .ToList();
    }

    if (!string.IsNullOrWhiteSpace(state))
    {
        issues = issues
            .Where(i => i.GetProperty("state").GetString().Equals(state, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    return Results.Json(issues);
});



// app.MapGet("/github-contributions", async (string? language, string? label, int? limit) =>
// {
//     var query = "state:open"; // Base query to only show open issues

//     // Add label filter
//     if (!string.IsNullOrWhiteSpace(label))
//     {
//         query += $" label:\"{Uri.EscapeDataString(label)}\"";
//     }
//     else
//     {
//         query += " label:\"good first issue\"";
//     }

//     // Add language filter
//     if (!string.IsNullOrWhiteSpace(language))
//     {
//         query += $" language:{Uri.EscapeDataString(language)}";
//     }

//     // URL to search GitHub issues
//     var url = $"https://api.github.com/search/issues?q={query}&per_page={limit ?? 10}";

//     using var client = new HttpClient();
//     client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyApp", "1.0"));
//     client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "YOUR_PERSONAL_ACCESS_TOKEN");

//     var response = await client.GetAsync(url);
//     if (!response.IsSuccessStatusCode) return Results.StatusCode((int)response.StatusCode);

//     var jsonResponse = await response.Content.ReadAsStringAsync();
//     return Results.Content(jsonResponse, "application/json");
// });



app.MapGet("/search-issues", async (
    string query,
    bool? goodFirstIssue = false,
    bool? helpWanted = false,
    string? createdAfter = null,
    string? updatedAfter = null,
    int? minOpenIssues = null
) =>
{
    if (string.IsNullOrWhiteSpace(query))
    {
        return Results.BadRequest("Query parameter is required.");
    }

    string filter = $"{query} is:issue is:open";
    if (goodFirstIssue == true) filter += " label:\"good first issue\"";
    if (helpWanted == true) filter += " label:\"help wanted\"";
    if (minOpenIssues.HasValue) filter += $" open_issues:>{minOpenIssues.Value}";
    if (!string.IsNullOrWhiteSpace(createdAfter)) filter += $" created:>{createdAfter}";
    if (!string.IsNullOrWhiteSpace(updatedAfter)) filter += $" updated:>{updatedAfter}";

    var url = $"https://api.github.com/search/issues?q={Uri.EscapeDataString(filter)}";

    using var client = new HttpClient();
    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("App", "1.0"));

    var response = await client.GetAsync(url);
    if (!response.IsSuccessStatusCode)
    {
        return Results.StatusCode((int)response.StatusCode);
    }

    try
    {
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

        return Results.Ok(new
        {
            TotalCount = rootElement.GetProperty("total_count").GetInt32(),
            Items = issues
        });
    }
    catch (Exception ex)
    {
        return Results.Problem($"An error occurred: {ex.Message}");
    }
});



app.Run();

