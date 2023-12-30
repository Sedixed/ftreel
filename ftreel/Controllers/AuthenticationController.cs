﻿using System.Security.Claims;
using ftreel.Constants;
using ftreel.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using AuthenticationService = ftreel.Services.AuthenticationService;

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
    public IActionResult GetUser()
    {
        return Ok(Json(User.Identity?.Name));
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
            new Claim(ClaimTypes.Name, user.Username),
            //new Claim("FullName", user.FullName),
            new Claim(ClaimTypes.Role, Roles.ROLE_ADMIN),
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
        
        return Ok(Json(User.Identity?.Name));
    }

    /**
     * Login request containing a username and a password.
     */
    public record LoginRequest(string Username, string Password);
    
    /**
     * Register request containing a username, a password and a list of roles.
     */
    public record RegisterRequest(string Username, string Password, IList<string> Roles);
}