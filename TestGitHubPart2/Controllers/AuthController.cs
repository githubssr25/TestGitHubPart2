


using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace postMVPFinalProject.Controllers
{

// [ApiController]
//     [Route("api/[controller]")]
//     public class AuthController : ControllerBase
//     {
//         private readonly GitHubPostMVPDbContext _dbContext;
//         private readonly UserManager<User> _userManager;

//         public AuthController(GitHubPostMVPDbContext dbContext, UserManager<User> userManager)
//         {
//             _dbContext = dbContext;
//             _userManager = userManager;
//         }

// [HttpPost("login")]
// public async Task<IActionResult> Login([FromHeader(Name = "Authorization")] string authHeader)
// {
//     // Step 1: Local Login
//     if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Basic "))
//         return Unauthorized("Invalid Authorization header.");

//     string encodedCreds = authHeader.Substring(6).Trim();
//     string creds = Encoding.GetEncoding("iso-8859-1").GetString(Convert.FromBase64String(encodedCreds));

//     int separator = creds.IndexOf(':');
//     if (separator == -1)
//         return Unauthorized("Invalid credentials format.");

//     string email = creds.Substring(0, separator);
//     string password = creds.Substring(separator + 1);

//     var user = await _userManager.FindByEmailAsync(email);
//     if (user == null)
//         return Unauthorized("Invalid credentials.");

//     var result = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
//     if (result != PasswordVerificationResult.Success)
//         return Unauthorized("Invalid credentials.");

//     var claims = new List<Claim>
//     {
//         new Claim(ClaimTypes.NameIdentifier, user.Id),
//         new Claim(ClaimTypes.Email, user.Email),
//         new Claim(ClaimTypes.Name, user.UserName)
//     };

//     var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

//     await HttpContext.SignInAsync(
//         CookieAuthenticationDefaults.AuthenticationScheme,
//         new ClaimsPrincipal(claimsIdentity));

//     // Step 2: Trigger GitHub OAuth Login
//     var clientId = _config["GitHub:OAuth:ClientId"];
//     var redirectUri = _config["GitHub:OAuth:RedirectUri"];
//     var scope = "repo";

//     var authorizationUrl = $"https://github.com/login/oauth/authorize?client_id={clientId}&redirect_uri={redirectUri}&scope={scope}";

//     return Ok(new { message = "Login successful. Redirecting to GitHub for OAuth.", redirectToGitHub = authorizationUrl });
// }

















// }


}