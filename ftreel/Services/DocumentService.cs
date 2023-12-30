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
}