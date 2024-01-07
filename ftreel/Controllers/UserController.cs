using ftreel.Dto.user;
using ftreel.Exceptions;
using ftreel.Services;
using Microsoft.AspNetCore.Mvc;

namespace ftreel.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UserController : Controller
{
    private readonly ILogger _logger;
    
    private readonly IUserService _userService;

    public UserController(ILogger<UserController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }
    
    /**
     * Get a user.
     */
    //[CustomAuthorize]
    [HttpGet("{id:int}")]
    //[Authorize]
    public IActionResult GetUser(int id) {
        try
        {
            var user = _userService.FindUser(id);
            return Ok(new UserDTO(user));
        }
        catch (ObjectNotFoundException e)
        {
            return NotFound();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /**
     * Get all users.
     */
    [HttpGet]
    //[Authorize(Roles="ROLE_ADMIN")]
    public IActionResult GetAllUsers()
    {
        try
        {
            var users = _userService.FindAllUser();
            IList<UserDTO> dtos = users.Select(user => new UserDTO(user)).ToList();

            return Ok(dtos);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    /**
     * Create a user.
     */
    [HttpPost]
    public IActionResult CreateUser(SaveUserDTO request)
    {
        try
        {
            var user = _userService.CreateUser(request);
            return Ok(new UserDTO(user));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    

    /**
     * Update a user.
     */
    [HttpPatch("{id:int}")]
    public IActionResult UpdateUser(int id, SaveUserDTO request) {
        try
        {
            var user = _userService.UpdateUser(id, request);
            return Ok(new UserDTO(user));
        }
        catch (ObjectNotFoundException e)
        {
            return NotFound();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    /**
     * Delete a file.
     */
    [HttpDelete("{id:int}")]
    public IActionResult DeleteUser(int id) {
        try
        {
            _userService.DeleteUser(id);
            return NoContent();
        }
        catch (ObjectNotFoundException e)
        {
            return NotFound();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}