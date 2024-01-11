using ftreel.Dto.category;
using ftreel.Dto.user;
using ftreel.Entities;
using ftreel.Exceptions;
using ftreel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> GetCategory(int id)
    {
        try
        {
            var category = _categoryService.FindCategory(id);
            return Ok(new CategoryDTO(category));
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
    
    [HttpGet]
    public IActionResult GetCategoryWithPath(string path = "/", string filter = "", string value = "")
    {
        try
        {
            var category = _categoryService.FindCategoryWithPath(path, filter, value);
            return Ok(new CategoryDTO(category));
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
     * Get all files.
     */
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        try
        {
            var categories = _categoryService.FindAllCategories();
            IList<CategoryDTO> dtos = new List<CategoryDTO>();
            foreach (var category in categories)
            {
                dtos.Add(new CategoryDTO(category));
            }
            return Ok(dtos);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    /**
     * Create a category.
     */
    [HttpPost]
    [Authorize(Roles = "ROLE_ADMIN")]
    public async Task<IActionResult> CreateCategory(SaveCategoryDTO request)
    {
        try
        {
            var category = _categoryService.CreateCategory(request);
            return Ok(new CategoryDTO(category));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    /**
     * Update a category.
     */
    [HttpPatch]
    [Authorize(Roles = "ROLE_ADMIN")]
    public async Task<IActionResult> UpdateCategory(SaveCategoryDTO request) {
        try
        {
            var category = _categoryService.UpdateCategory(request);
            return Ok(new CategoryDTO(category));
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
     * Delete a category.
     */
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "ROLE_ADMIN")]
    public async Task<IActionResult> DeleteCategory(int id) {
        try
        {
            _categoryService.DeleteCategory(id);
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

    /**
     * Subscribe current logged user.
     */
    [HttpPost("{id:int}")]
    public IActionResult SubscribeCategory(int id)
    {
        try
        {
            _categoryService.SubscribeCategory(id, User.Identity);
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
    
    /**
     * Unsubscribe current logged user.
     */
    [HttpPost("{id:int}")]
    public IActionResult UnsubscribeCategory(int id)
    {
        try
        {
            _categoryService.UnsubscribeCategory(id, User.Identity);
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

    /**
     * Get followed categories of the current logged user.
     */
    [HttpGet]
    public IActionResult GetFollowedCategories()
    {
        try
        {
            var user = _authenticationService.GetAuthenticatedUser(User.Identity);
            return Ok(new FollowedCategoriesDTO(user));
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