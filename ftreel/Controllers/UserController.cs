using ftreel.Dto.user;
using ftreel.Exceptions;
using ftreel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ftreel.Dto.error;

namespace ftreel.Controllers;

[ApiController]
[Authorize(Roles = "ROLE_ADMIN")]
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
    [HttpGet("{id:int}")]
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
            return BadRequest(new ErrorDTO(e.Message));
        }
    }

    /**
     * Get all users.
     */
    [HttpGet]
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
            return BadRequest(new ErrorDTO(e.Message));
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
            return BadRequest(new ErrorDTO(e.Message));
        }
    }

    

    /**
     * Update a user.
     */
    [HttpPatch]
    public IActionResult UpdateUser(SaveUserDTO request) {
        try
        {
            var user = _userService.UpdateUser(request);
            return Ok(new UserDTO(user));
        }
        catch (ObjectNotFoundException e)
        {
            return NotFound();
        }
        catch (Exception e)
        {
            return BadRequest(new ErrorDTO(e.Message));
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
            return BadRequest(new ErrorDTO(e.Message));
        }
    }
}