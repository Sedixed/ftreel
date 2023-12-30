using ftreel.DATA;
using ftreel.Dto.document;
using ftreel.Entities;
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

    private readonly DocumentService _documentService;

    public DocumentController(ILogger<DocumentController> logger, AppDBContext dbcontext, DocumentService documentService)
    {
        _dbcontext = dbcontext;
        _logger = logger;
        _documentService = documentService;
    }

    /**
     * Upload a file
     */
    [HttpPost]
    public async Task<IActionResult> UploadDocument(SaveDocumentDTO request)
    {
        var user = _documentService.Save(request);
        
        _logger.LogInformation(request.Title + " file created.");
        return Ok(user);
    }

    /**
     * Get a file.
     */
    [HttpGet("{id:int}")]
    public async Task<IActionResult> DownloadFile(int id) {
        var document = await _dbcontext.Documents.FindAsync(id);

        if (document == null)
            return NotFound();

        var memory = new MemoryStream();

        using (var stream = new FileStream(document.FilePath, FileMode.Open))
            await stream.CopyToAsync(memory);

        memory.Position = 0;

        return File(memory, UtilsClass.GetContentType(document.FilePath), document.Title);
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

    /**
     * Get a file.
     */
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetFile(int id) {
        var document = await _dbcontext.Documents.FindAsync(id);

        if (document == null)
            return NotFound();

        using (var stream = new FileStream(document.FilePath, FileMode.Open))
        {
            // Open a navigator tab to visualize the document 
        }

        return Ok();
    }
}
