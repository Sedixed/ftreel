using ftreel.DATA;
using ftreel.Dto.document;
using ftreel.Entities;

namespace ftreel.Services;

public class DocumentService
{
    private readonly FileSystemStorageService _fileSystemStorageService;
    private readonly AppDBContext _dbContext;

    public DocumentService(FileSystemStorageService fileSystemStorageService, AppDBContext dbContext)
    {
        _fileSystemStorageService = fileSystemStorageService;
        _dbContext = dbContext;
    }
    
    /**
     * Save a file in database and in storage.
     */
    public Document Save(SaveDocumentDTO uploadRequest)
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
}