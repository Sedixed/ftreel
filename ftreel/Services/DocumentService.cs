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
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
        }
        
        return document;
    }

    public IList<Document> FindAllDocuments()
    {
        throw new NotImplementedException();
    }

    /**
     * Save a file in database and in storage system.
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

        Console.WriteLine(document.Id);
        Console.WriteLine(document.Title);
        _fileSystemStorageService.store(document);

        return document;
    }

    public Document UpdateDocument(SaveDocumentDTO updateRequest)
    {
        throw new NotImplementedException();
    }

    public void DeleteDocument(int id)
    {
        throw new NotImplementedException();
    }
}