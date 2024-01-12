using ftreel.Dto.category;
using ftreel.Dto.user;
using ftreel.Exceptions;
using ftreel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ftreel.Dto.error;

namespace ftreel.Controllers;

/**
 * Controller to manage categories.
 */
[ApiController]
[Authorize]
[Route("[controller]/[action]")]
public class CategoryController : Controller
{
    private readonly ILogger _logger;

    private readonly ICategoryService _categoryService;

    private readonly AuthenticationService _authenticationService;

    public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService, AuthenticationService authenticationService)
    {
        _logger = logger;
        _categoryService = categoryService;
        _authenticationService = authenticationService;
    }

    /**
     * Get a file.
     */
    [HttpGet("{id:int}")]
    public IActionResult GetCategory(int id)
    {
        try
        {
            var category = _categoryService.FindCategory(id);
            _logger.LogInformation("Category {Name} retrieved with ID at {Time} by user {User}.", 
                category.Name, DateTime.UtcNow, User.Identity?.Name);
            return Ok(new CategoryDTO(category, _authenticationService.GetAuthenticatedUser(User.Identity)));
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
    
    [HttpGet]
    public IActionResult GetCategoryWithPath(string path = "/", string filter = "", string value = "")
    {
        try
        {
            var category = _categoryService.FindCategoryWithPath(path, filter, value, User.Identity);
            _logger.LogInformation("Category {Name} retrieved with path at {Time} by user {User}.", 
                category.Name, DateTime.UtcNow, User.Identity?.Name);
            return Ok(new CategoryDTO(category, _authenticationService.GetAuthenticatedUser(User.Identity)));
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
     * Get all files.
     */
    [HttpGet]
    public IActionResult GetAllCategories()
    {
        try
        {
            var categories = _categoryService.FindAllCategories();
            _logger.LogInformation("All categories retrieved at {Time} by user {User}.", 
                DateTime.UtcNow, User.Identity?.Name);
            IList<CategoryDTO> dtos = new List<CategoryDTO>();
            foreach (var category in categories)
            {
                dtos.Add(new CategoryDTO(category, _authenticationService.GetAuthenticatedUser(User.Identity)));
            }
            return Ok(dtos);
        }
        catch (Exception e)
        {
            return BadRequest(new ErrorDTO(e.Message));
        }
    }
    
    /**
     * Create a category.
     */
    [HttpPost]
    [Authorize(Roles = "ROLE_ADMIN")]
    public IActionResult CreateCategory(SaveCategoryDTO request)
    {
        try
        {
            var category = _categoryService.CreateCategory(request);
            _logger.LogInformation("Category {Name} created at {Time} by user {User}.", 
                category.Name, DateTime.UtcNow, User.Identity?.Name);
            return Ok(new CategoryDTO(category, _authenticationService.GetAuthenticatedUser(User.Identity)));
        }
        catch (Exception e)
        {
            return BadRequest(new ErrorDTO(e.Message));
        }
    }
    
    /**
     * Update a category.
     */
    [HttpPatch]
    [Authorize(Roles = "ROLE_ADMIN")]
    public IActionResult UpdateCategory(SaveCategoryDTO request) {
        try
        {
            var category = _categoryService.UpdateCategory(request);
            _logger.LogInformation("Category {Name} updated at {Time} by user {User}.", 
                category.Name, DateTime.UtcNow, User.Identity?.Name);
            return Ok(new CategoryDTO(category, _authenticationService.GetAuthenticatedUser(User.Identity)));
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
     * Delete a category.
     */
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "ROLE_ADMIN")]
    public IActionResult DeleteCategory(int id) {
        try
        {
            _categoryService.DeleteCategory(id);
            _logger.LogInformation("Category with ID {Id} deleted at {Time} by user {User}.", 
                id, DateTime.UtcNow, User.Identity?.Name);
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

    /**
     * Subscribe current logged user.
     */
    [HttpPost("{id:int}")]
    public IActionResult SubscribeCategory(int id)
    {
        try
        {
            _categoryService.SubscribeCategory(id, User.Identity);
            _logger.LogInformation("User {User} subscribed to category with ID {Id} at {Time}.", 
                User.Identity?.Name, id, DateTime.UtcNow);
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
    
    /**
     * Unsubscribe current logged user.
     */
    [HttpPost("{id:int}")]
    public IActionResult UnsubscribeCategory(int id)
    {
        try
        {
            _categoryService.UnsubscribeCategory(id, User.Identity);
            _logger.LogInformation("User {User} unsubscribed to category with ID {Id} at {Time}.", 
                User.Identity?.Name, id, DateTime.UtcNow);
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

    /**
     * Get followed categories of the current logged user.
     */
    [HttpGet]
    public IActionResult GetFollowedCategories()
    {
        try
        {
            var user = _authenticationService.GetAuthenticatedUser(User.Identity);
            _logger.LogInformation("All followed categories of user {User} retrieved at {Time}.", 
                User.Identity?.Name, DateTime.UtcNow);
            return Ok(new FollowedCategoriesDTO(user));
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