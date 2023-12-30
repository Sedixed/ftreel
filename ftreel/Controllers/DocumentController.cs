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
     * Upload a file
     */
    [HttpPost]
    public async Task<IActionResult> UploadDocument(SaveDocumentDTO request)
    {
        var user = _documentService.SaveDocument(request);
        
        _logger.LogInformation(request.Title + " file created.");
        return Ok(user);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteFile(int id) {
        var document = await _dbcontext.Documents.FindAsync(id);

        if (document == null)
            return NotFound();

        _dbcontext.Documents.Remove(document);
        await _dbcontext.SaveChangesAsync();

        System.IO.File.Delete(document.FilePath);

        return Ok();
    }

    /**
     * Update a file.
     */
    [HttpPatch("{id:int}")]
    // Je souhaite update les infos du document, pas le document en lui-même
    public async Task<IActionResult> UpdateFile(int id, Document updatedDocument) {
        if (id != updatedDocument.Id)
            return BadRequest();

        var existingDocument = await _dbcontext.Documents.FindAsync(id);

        if (existingDocument == null)
            return NotFound();

        // Mettez à jour uniquement les propriétés non nulles

        // TODO : Trouver un moyen de ne pas exposer le Document en paramètre de la méthode
        // Hint : Faire un DTO
        foreach (var property in typeof(Document).GetProperties())
        {
            var updatedValue = property.GetValue(updatedDocument);
            if (updatedValue != null && String.IsNullOrEmpty(updatedValue.ToString()) == false)
            {
                property.SetValue(existingDocument, updatedValue);
            }
        }

        await _dbcontext.SaveChangesAsync();
        
        return Ok();
    }
}
