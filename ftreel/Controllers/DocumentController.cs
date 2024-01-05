using ftreel.Annotations;
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

    public DocumentController(ILogger<DocumentController> logger, IDocumentService documentService)
    {
        _logger = logger;
        _documentService = documentService;
    }
    
    /**
     * Get a file.
     */
    //[CustomAuthorize]
    [HttpGet("{id:int}")]
    public IActionResult GetDocument(int id) {
        try
        {
            var document = _documentService.FindDocument(id);
            return Ok(new DocumentDTO(document));
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
    //[CustomAuthorize(Roles="ROLE_ADMIN")]
    public IActionResult GetAllDocuments()
    {
        try
        {
            var documents = _documentService.FindAllDocuments();
            IList<DocumentDTO> dtos = documents.Select(document => new DocumentDTO(document)).ToList();

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
    public async Task<IActionResult> UploadDocument(SaveDocumentDTO request)
    {
        try
        {
            var document = _documentService.SaveDocument(request);
            _logger.LogInformation(request.Title + " file created.");
            return Ok(new DocumentDTO(document));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    

    /**
     * Update a file.
     */
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateDocument(int id, SaveDocumentDTO request) {
        try
        {
            var document = _documentService.UpdateDocument(id, request);
            _logger.LogInformation(request.Title + " file updated.");
            return Ok(new DocumentDTO(document));
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
    public async Task<IActionResult> DeleteDocument(int id) {
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
}
