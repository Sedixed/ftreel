﻿using ftreel.Dto.category;
using ftreel.Dto.user;
using ftreel.Entities;
using ftreel.Exceptions;
using ftreel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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