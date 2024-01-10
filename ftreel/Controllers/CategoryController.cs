using ftreel.Dto.category;
using ftreel.Entities;
using ftreel.Exceptions;
using ftreel.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ftreel.Controllers;

/**
 * Controller to manage categories.
 */
[ApiController]
[Route("[controller]/[action]")]
public class CategoryController : Controller
{
    private readonly ILogger _logger;

    private readonly ICategoryService _categoryService;

    public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService)
    {
        _logger = logger;
        _categoryService = categoryService;
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
    public async Task<IActionResult> GetCategoryWithPath(string path = "/")
    {
        try
        {
            var category = _categoryService.FindCategoryWithPath(path);
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
}