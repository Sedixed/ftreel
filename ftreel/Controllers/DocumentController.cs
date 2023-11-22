using ftreel.DATA;
using ftreel.Settings;
using ftreel.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ftreel.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly AppDBContext _dbcontext;

        private readonly UploadSettings _opt;

        public DocumentController(AppDBContext dbcontext, IOptions<UploadSettings> opt)
        {
            _opt = opt.Value;
            _dbcontext = dbcontext;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            // I want to log what "file" is to the console
            Console.WriteLine(file.ToString());
            
            var uploadPath = _opt.FilePath;

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream);

            var newDocument = new Document
            {
                Title = file.FileName,
                Description = "This is a test document",
                FilePath = filePath,
                Extension = Path.GetExtension(filePath),
                Author = "Test Author",
                Category = "Test Category"
            };

            _dbcontext.Documents.Add(newDocument);
            await _dbcontext.SaveChangesAsync();

            // On return, give the id of the new document in DB
            return Ok(newDocument.Id);
        }

        [HttpGet("download/{id}")]
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

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteFile(int id) {
            var document = await _dbcontext.Documents.FindAsync(id);

            if (document == null)
                return NotFound();

            _dbcontext.Documents.Remove(document);
            await _dbcontext.SaveChangesAsync();

            System.IO.File.Delete(document.FilePath);

            return Ok();
        }

        [HttpPatch("update/{id}")]
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

        [HttpGet("get/{id}")]
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
}
