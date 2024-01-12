using System.Security.Claims;
using ftreel.Constants;
using ftreel.Dto.user;
using ftreel.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using AuthenticationService = ftreel.Services.AuthenticationService;
using ftreel.Dto.error;

namespace ftreel.Controllers;

/**
 * Controller containing authentication routes.
 */
[ApiController]
[Route("[controller]/[action]")]
public class AuthenticationController : Controller
{
    private readonly ILogger _logger;
    private readonly AuthenticationService _authenticationService;

    public AuthenticationController(ILogger<AuthenticationController> logger, AuthenticationService authenticationService)
    {
        _logger = logger;
        _authenticationService = authenticationService;
    }
    
    /**
     * Login route.
     */
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = _authenticationService.AuthenticateUser(request.Username, request.Password);

        var result = await SignIn(user);
        return result;
    }
    
    /**
     * Logout route.
     */
    [HttpPost]
    [Authorize]
    public async void Logout()
    {
        _logger.LogInformation("User {Name} logged out at {Time}.", 
            User.Identity?.Name, DateTime.UtcNow);
            
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    /**
     * Register new user route.
     */
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = _authenticationService.RegisterUser(request.Username, request.Password, request.Roles);
        if (user == null)
        {
            return Unauthorized(Json("User with username " + request.Username + " already exists."));
        }
        
        var result = await SignIn(user);
        return result;
    }

    /**
     * Get current user logged.
     */
    [HttpGet]
    [Authorize]
    public IActionResult GetUser()
    {
        return Ok(new UserDTO(_authenticationService.GetAuthenticatedUser(User.Identity)));
    }

    /**
     * Private function to sign in the user authenticated.
     */
    
    private async Task<IActionResult> SignIn(User? user)
    {
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Unauthorized();
        }

        var claims = new List<Claim>
        {
            // User data.
            new Claim(ClaimTypes.Name, user.Mail),
            //new Claim("FullName", user.FullName),
        };
        
        // User roles.
        if (user.Roles != null) claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties{};

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        _logger.LogInformation("User {username} logged in at {Time}.",
            user.Mail, DateTime.UtcNow);
        
        return Ok(new UserDTO(user));
    }

    /**
     * Login request containing a username and a password.
     */
    public record LoginRequest(string Username, string Password);
    
    /**
     * Register request containing a username, a password and a list of roles.
     */
    public record RegisterRequest(string Username, string Password, IList<Roles> Roles);
}