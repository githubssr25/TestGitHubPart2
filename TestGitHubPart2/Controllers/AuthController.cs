using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using PostMVPFinalProject.Context;
using PostMVPProject.Models;
using PostMVPProject.Models.DTOs;
using System.Net.Http.Headers;

namespace postMVPFinalProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly GitHubPostMVPDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<User> _userManager;

        public AuthController(
            GitHubPostMVPDbContext dbContext,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _userManager = userManager;
        }

        // Step 1: Login with email/password and authenticate GitHub in the background
[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
{
    try
    {
        Console.WriteLine($"Login attempt for email: {loginDto.Email}");

        if (string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
        {
               Console.WriteLine("Missing email or password");
            return BadRequest("Email and password are required.");
        }

        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            Console.WriteLine("User not found");
            return Unauthorized("Invalid credentials.");
        }

        var result = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
        if (result != PasswordVerificationResult.Success)
        {
             Console.WriteLine("Password verification failed");
            return Unauthorized("Invalid credentials.");
        }

          Console.WriteLine("User authenticated successfully");

        // Authenticate user locally
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity)
        );

        // Automatically trigger GitHub authentication if user doesn't have a GitHub token yet
        if (string.IsNullOrEmpty(user.GitHubToken))
        {
        Console.WriteLine("GitHub token missing, redirecting to GitHub auth");
        var clientId = _configuration["GitHub:ClientId"];
        var redirectUri = _configuration["GitHub:RedirectUri"];
        var scope = "repo user";
        var state = Guid.NewGuid().ToString(); // Add state parameter for security

        // Store state in session
        HttpContext.Session.SetString("GitHubState", state);

        Console.WriteLine($"Generated state: {state}");
        HttpContext.Session.SetString("GitHubState", state);

        var githubAuthUrl = $"https://github.com/login/oauth/authorize" +
                            $"?client_id={clientId}" +
                            $"&redirect_uri={redirectUri}" +
                            $"&scope={scope}" +
                            $"&state={state}";

            return Ok(new
            {
                id = user.Id,
                email = user.Email,
                userName = user.UserName,
                githubAuthUrl,
                message = "Login successful. Redirect to GitHub for authentication."
            });
        }
        //This checks if the user already has a GitHub token stored in the database. If they don't, it sends a URL to the frontend for GitHub OAuth login.

//         What's Happening in Your Flow?
// User logs in with email/password.
// If they haven't authenticated with GitHub, the backend returns a githubAuthUrl for them to visit.
// The user is redirected to GitHub and grants permissions. THIS IS THE HELPER METHOD PART AT THE BOTTOM 
// GitHub redirects back to your app with an authorization code.
// The backend calls ExchangeGitHubCodeForToken() to get an access token and stores it.

        return Ok(new
        {
            id = user.Id,
            email = user.Email,
            userName = user.UserName,
            message = "Login successful."
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during login: {ex.Message}");
        return StatusCode(500, $"An error occurred: {ex.Message}");
    }
}


[HttpPost("github-auth")]
public async Task<IActionResult> GitHubAuth([FromBody] LoginDTO loginDto)
{
    if (string.IsNullOrEmpty(loginDto.GitHubCode))
    {
        return BadRequest("GitHub authorization code is required.");
    }

    var githubToken = await ExchangeGitHubCodeForToken(loginDto.GitHubCode);
    if (githubToken == null)
    {
        return Unauthorized("Failed to authenticate with GitHub.");
    }

    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var user = await _userManager.FindByIdAsync(userId);
    if (user == null)
    {
        return Unauthorized("User not found.");
    }

    // Store GitHub token securely
    user.GitHubToken = githubToken;
    await _userManager.UpdateAsync(user);

    return Ok(new
    {
        message = "GitHub authentication successful",
        githubToken
    });
}


[HttpGet("github-login")]
public IActionResult GitHubLogin()
{
    var clientId = _configuration["GitHub:ClientId"];
    var redirectUri = _configuration["GitHub:RedirectUri"];
    var scope = "repo user";  // Define GitHub permissions here

    var githubAuthUrl = $"https://github.com/login/oauth/authorize" +
                        $"?client_id={clientId}" +
                        $"&redirect_uri={redirectUri}" +
                        $"&scope={scope}";

    return Redirect(githubAuthUrl);
}

[HttpGet("github-callback")]
public async Task<IActionResult> GitHubCallback([FromQuery] string code, [FromQuery] string state)
{

    // Verify state to prevent CSRF
    var savedState = HttpContext.Session.GetString("GitHubState");
    Console.WriteLine($"Saved state: {savedState}, Received state: {state}");
    if (string.IsNullOrEmpty(state) || state != savedState)
    {
        return BadRequest("Invalid state parameter");
    }

    var githubToken = await ExchangeGitHubCodeForToken(code);
    if (githubToken == null)
    {
        return Unauthorized("Failed to authenticate with GitHub.");
    }

    // Get current user
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var user = await _userManager.FindByIdAsync(userId);
    if (user == null)
    {
        return Unauthorized("User not found.");
    }

    // Store GitHub token
    user.GitHubToken = githubToken;
    await _userManager.UpdateAsync(user);

    // Clear the state from session
    HttpContext.Session.Remove("GitHubState");

    // Return success response that frontend can handle
    // return Ok(new { 
    //     message = "GitHub authentication successful",
    //     userId = user.Id
    // });
    return Redirect($"http://localhost:5173");
}


private async Task<string?> ExchangeGitHubCodeForToken(string code)
//This helper method is called when GitHub redirects back to the app with an authorization code. 
//The backend exchanges this code for an access token and stores it for future API calls.
{
     try
    {
        var clientId = _configuration["GitHub:ClientId"];
        var clientSecret = _configuration["GitHub:ClientSecret"];
        var redirectUri = _configuration["GitHub:RedirectUri"];

        using var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var tokenRequest = new Dictionary<string, string>
        {
            { "client_id", clientId },
            { "client_secret", clientSecret },
            { "code", code },
            { "redirect_uri", redirectUri }
        };

        var response = await client.PostAsync(
            "https://github.com/login/oauth/access_token",
            new FormUrlEncodedContent(tokenRequest));

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"GitHub returned: {error}");
        }

        var result = await response.Content.ReadAsStringAsync();
        var token = JsonSerializer.Deserialize<GitHubTokenResponse>(result);
        return token?.access_token;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"GitHub token exchange failed: {ex.Message}");
        return null;
    }
}


    }
}