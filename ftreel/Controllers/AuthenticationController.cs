using System.Security.Claims;
using ftreel.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace ftreel.Controllers;

/**
 * Controller containing authentication routes.
 */
[ApiController]
[Route("[controller]/[action]")]
public class AuthenticationController : Controller
{

    private readonly ILogger _logger;

    public AuthenticationController(ILogger<AuthenticationController> logger)
    {
        _logger = logger;
    }
    
    /**
     * Login route.
     */
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (ModelState.IsValid)
        {
            var user = await AuthenticateUser(request.Username, request.Password);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                //new Claim("FullName", user.FullName),
                new Claim(ClaimTypes.Role, "Administrator"),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties{};
            

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);


            _logger.LogInformation("User {username} logged in at {Time}.",
                user.Username, DateTime.UtcNow);
        }

        return NoContent();
    }

    
    /**
     * Logout route.
     */
    [HttpPost]
    public async void Logout()
    {
        _logger.LogInformation("User {Name} logged out at {Time}.", 
            User.Identity.Name, DateTime.UtcNow);
            
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    /**
     * Register new user route.
     */
    [HttpPost]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        return NoContent();
    }

    /**
     * Get current user logged.
     */
    [HttpGet]
    public IActionResult GetUser()
    {
        return Ok(Json(User.Identity.Name));
    }
    
    /**
     * Private method to authenticate a user.
     * Returns the user authenticated, or null if it does not exists in database.
     */
    private async Task<User> AuthenticateUser(string username, string password)
    {
        await Task.Delay(500);

        if (username == "username") {
            return new User()
            {
                Username = username,
                Password = password
            };
        } else {
            return null;
        }
    }

    public record LoginRequest(string Username, string Password);
    public record RegisterRequest(string Username, string Password, List<String> Roles);
}