using ftreel.DATA;
using ftreel.Dto.document;
using ftreel.Entities;
using ftreel.Exceptions;
using ftreel.Services;
using ftreel.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ftreel.Controllers;

/**
 * Controller to manage documents.
 */
[ApiController]
[Route("[controller]/[action]")]
public class DocumentController : Controller
{
    private readonly ILogger _logger;
    
    private readonly AppDBContext _dbcontext;

    private readonly IDocumentService _documentService;

    public DocumentController(ILogger<DocumentController> logger, AppDBContext dbcontext, IDocumentService documentService)
    {
        _logger = logger;
        _dbcontext = dbcontext;
        _documentService = documentService;
    }
    
    /**
     * Get a file.
     */
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDocument(int id) {
        try
        {
            var document = _documentService.FindDocument(id);
            return Ok(document);
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
    public async Task<IActionResult> GetAllDocuments()
    {
        try
        {
            var documents = _documentService.FindAllDocuments();
            return Ok(documents);
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
    public async Task<IActionResult> UploadDocument([FromBody] SaveDocumentDTO request)
    {
        try
        {
            var document = _documentService.SaveDocument(request);
            _logger.LogInformation(request.Title + " file created.");
            return Ok(document);
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
    public async Task<IActionResult> UpdateDocument(int id, [FromBody] SaveDocumentDTO request) {
        try
        {
            var document = _documentService.UpdateDocument(id, request);
            _logger.LogInformation(request.Title + " file updated.");
            return Ok(document);
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
