using ftreel.DATA;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
namespace ftreel.Services;

public class AuthenticationService
{
    
    private readonly ILogger _logger;

    public AuthenticationService(ILogger<IAuthenticationService> logger)
    {
        _logger = logger;
    }
    
    public void Login(string username, string password)
    {
        
    }

    public void AuthenticatedUser()
    {
        throw new NotImplementedException();
    }
}