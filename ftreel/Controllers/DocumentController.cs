using ftreel.Dto.document;
using ftreel.Exceptions;
using ftreel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ftreel.Controllers;

/**
 * Controller to manage documents.
 */
[ApiController]
[Authorize]
[Route("[controller]/[action]")]
public class DocumentController : Controller
{
    private readonly ILogger _logger;
    
    private readonly IDocumentService _documentService;
    
    private readonly AuthenticationService _authenticationService;

    public DocumentController(ILogger<DocumentController> logger, IDocumentService documentService, AuthenticationService authenticationService)
    {
        _logger = logger;
        _documentService = documentService;
        _authenticationService = authenticationService;
    }
    
    /**
     * Get a file.
     */
    [HttpGet("{id:int}")]
    public IActionResult GetDocument(int id) {
        try
        {
            var document = _documentService.FindDocument(id);
            return Ok(new DocumentDTO(document, _authenticationService.GetAuthenticatedUser(User.Identity)));
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
    public IActionResult GetAllDocuments()
    {
        try
        {
            var documents = _documentService.FindAllDocuments();
            IList<DocumentDTO> dtos = documents.Select(document => new DocumentDTO(document, _authenticationService.GetAuthenticatedUser(User.Identity))).ToList();

            return Ok(dtos);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    /**
     * Upload a file.
     */
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UploadDocument(SaveDocumentDTO request)
    {
        try
        {
            var document = _documentService.SaveDocument(request, User.Identity);
            _logger.LogInformation(request.Title + " file created.");
            return Ok(new DocumentDTO(document, _authenticationService.GetAuthenticatedUser(User.Identity)));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    

    /**
     * Update a file.
     */
    [HttpPatch]
    //[Authorize(Roles = "ROLE_ADMIN")]
    public IActionResult UpdateDocument(SaveDocumentDTO request) {
        try
        {
            var document = _documentService.UpdateDocument(request);
            _logger.LogInformation(request.Title + " file updated.");
            return Ok(new DocumentDTO(document, _authenticationService.GetAuthenticatedUser(User.Identity)));
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
    [Authorize(Roles = "ROLE_ADMIN")]
    public IActionResult DeleteDocument(int id) {
        try
        {
            _documentService.DeleteDocument(id);
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
     * Get all document that are not validated
     */
    [HttpGet]
    [Authorize(Roles = "ROLE_ADMIN")]
    public IActionResult GetNotValidatedDocuments() {
        try
        {
            var documents = _documentService.GetNotValidatedDocuments();
            IList<DocumentDTO> dtos = documents.Select(document => new DocumentDTO(document, _authenticationService.GetAuthenticatedUser(User.Identity))).ToList();
            return Ok(dtos);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    /**
     * Validate a document.
     */
    [HttpPost("{id:int}")]
    [Authorize(Roles = "ROLE_ADMIN")]
    public IActionResult ValidateDocument(int id) {
        try
        {
            _documentService.ValidateDocument(id, User.Identity);
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
     * Like a document.
     */
    [HttpPost("{id:int}")]
    public IActionResult Like(int id) {
        try
        {
            _documentService.LikeDocument(id, User.Identity);
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
     * Unlike a document.
     */
    [HttpPost("{id:int}")]
    public IActionResult Unlike(int id) {
        try
        {
            _documentService.UnlikeDocument(id, User.Identity);
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
