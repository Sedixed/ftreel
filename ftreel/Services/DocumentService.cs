using ftreel.DATA;
using ftreel.Dto.document;
using ftreel.Entities;
using ftreel.Exceptions;

namespace ftreel.Services;

public class DocumentService : IDocumentService
{
    private readonly ILogger _logger;
    private readonly IStorageService _fileSystemStorageService;
    private readonly AppDBContext _dbContext;

    public DocumentService(ILogger<DocumentService> logger, IStorageService fileSystemStorageService, AppDBContext dbContext)
    {
        _logger = logger;
        _fileSystemStorageService = fileSystemStorageService;
        _dbContext = dbContext;
    }

    /**
     * Find a file using its ID.
     */
    public Document FindDocument(int id)
    {
        var document = _dbContext.Documents.Find(id);

        if (document == null)
        {
            throw new ObjectNotFoundException();
        }
        
        try
        {
            _fileSystemStorageService.loadBase64(document);
        }
        catch (StorageException e)
        {
            _logger.LogInformation(e.Message);
        }
        
        return document;
    }

    /**
     * Find all file from database and storage system.
     */
    public IList<Document> FindAllDocuments()
    {
        var documents = _dbContext.Documents.ToList();

        foreach (var document in documents)
        {
            try
            {
                _fileSystemStorageService.loadBase64(document);
            }
            catch (StorageException e)
            {
                _logger.LogInformation(e.Message);
            }
        }

        return documents;
    }

    /**
     * Create a file in database and in storage system.
     */
    public Document SaveDocument(SaveDocumentDTO uploadRequest)
    {
        Document document = new Document(
            uploadRequest.Title,
            uploadRequest.Description,
            uploadRequest.FilePath,
            uploadRequest.Extension,
            uploadRequest.Author,
            uploadRequest.Category,
            uploadRequest.Base64
        );
        _dbContext.Add(document);
        _dbContext.SaveChanges();
        
        _fileSystemStorageService.store(document);

        return document;
    }

    /**
     * Update a file in database and in storage system.
     */
    public Document UpdateDocument(int id, SaveDocumentDTO updateRequest)
    {
        var document = _dbContext.Documents.Find(id);

        if (document == null)
        {
            throw new ObjectNotFoundException();
        }
        
        try
        {
            _fileSystemStorageService.loadBase64(document);
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
        }
        
        // If base 64 content has changed.
        if (updateRequest.Base64 != null && !updateRequest.Base64.Equals("") && document.Base64 != updateRequest.Base64)
        {
            Console.WriteLine("uiuiuiuiuiuiuiuiuiuiuiuiuiuiuiuiuiuiuiuiui");
            // If file name has changed.
            if (updateRequest.Title != null && !updateRequest.Title.Equals("") && document.Title != updateRequest.Title) {
                try
                {
                    _fileSystemStorageService.delete(document);
                }
                catch (Exception e)
                {
                    _logger.LogInformation(e.Message);
                }
                Console.WriteLine("a");
            }
            
            PatchDocumentData(document, updateRequest);
            _fileSystemStorageService.store(document);
            _dbContext.SaveChanges();
        }
        else
        {
            // If file name has changed.
            if (updateRequest.Title != null && !updateRequest.Title.Equals("") && document.Title != updateRequest.Title) {
                Console.WriteLine("owoowoowoowoowoowoowoowoowoowo");
                try
                {
                    _fileSystemStorageService.delete(document);
                }
                catch (Exception e)
                {
                    _logger.LogInformation(e.Message);
                }
                
                PatchDocumentData(document, updateRequest);
                _fileSystemStorageService.store(document);
                _dbContext.SaveChanges();
            }
            else
            {
                Console.WriteLine("zrjrzojnrzrzznrzzrrzlrz jrzlzrl");
                PatchDocumentData(document, updateRequest);
                _dbContext.SaveChanges();
            }
        }

        return document;
    }

    /**
     * Delete a file from database and storage system using its ID.
     */
    public void DeleteDocument(int id)
    {
        var document = _dbContext.Documents.Find(id);

        if (document == null)
        {
            throw new ObjectNotFoundException();
        }

        _dbContext.Documents.Remove(document);
        try
        {
            _fileSystemStorageService.delete(document);
        }
        catch (StorageException e)
        {
            _logger.LogInformation(e.Message);
        }

        _dbContext.SaveChanges();
    }

    private void PatchDocumentData(Document document, SaveDocumentDTO updateRequest)
    {
        if (!IsAttributeNull(updateRequest.Title))
        {
            document.Title = updateRequest.Title;
        }

        if (!IsAttributeNull(updateRequest.Description))
        {
            document.Description = updateRequest.Description;
        }

        if (!IsAttributeNull(updateRequest.FilePath))
        {
            document.FilePath = updateRequest.FilePath;
        }

        if (!IsAttributeNull(updateRequest.Extension))
        {
            document.Extension = updateRequest.Extension;
        }

        if (!IsAttributeNull(updateRequest.Author))
        {
            document.Author = updateRequest.Author;
        }

        if (!IsAttributeNull(updateRequest.Base64))
        {
            document.Base64 = updateRequest.Base64;
        }
    }

    private bool IsAttributeNull(string? attribute)
    {
        return attribute == null || attribute.Equals("");
    }
}