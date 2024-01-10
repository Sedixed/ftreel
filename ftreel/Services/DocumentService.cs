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
        // Create the document.
        var document = new Document(
            uploadRequest.Title,
            uploadRequest.Description,
            uploadRequest.ContentType,
            uploadRequest.Author,
            null,
            uploadRequest.Base64
        );

        // Associate to category.
        var parentCategory = _dbContext.Categories.Find(uploadRequest.CategoryId);
        CheckParentCategory(parentCategory, parentCategory?.Id, document.Title);
        document.CategoryId = parentCategory?.Id;
        document.Category = parentCategory;
        
        // Save in database.
        _dbContext.Add(document);
        _dbContext.SaveChanges();
        
        // Save a system storage.
        try
        {
            _fileSystemStorageService.store(document);
        }
        catch (Exception e)
        {
            _dbContext.Remove(document);
            _dbContext.SaveChanges();
            throw new Exception("Invalid base 64 content.");
        }

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
        
        // Update category.
        if (updateRequest.CategoryId != null && document.CategoryId != updateRequest.CategoryId)
        {
            if (updateRequest.CategoryId <= 0)
            {
                if (document.CategoryId != null) {
                    CheckParentCategory(null, null, document.Title);
                    document.Category = null;
                    document.CategoryId = null;
                }
            }
            else
            {
                var parentCategory = _dbContext.Categories.Find(updateRequest.CategoryId);
                CheckParentCategory(parentCategory, updateRequest.CategoryId, document.Title);
                document.Category = parentCategory;
                document.CategoryId = parentCategory?.Id;
            }
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
            var oldDocument = new Document()
            {
                Title = document.Title,
                Extension = document.Extension
            };
            PatchDocumentData(document, updateRequest);
            try
            {
                _fileSystemStorageService.store(document);
                // If file name has changed.
                if (updateRequest.Title != null && !updateRequest.Title.Equals("") && document.Title != updateRequest.Title)
                {
                    _fileSystemStorageService.delete(oldDocument);
                }
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                throw new Exception(e.Message);
            }
        }
        else
        {
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
                
                PatchDocumentData(document, updateRequest);
                try
                {
                    _fileSystemStorageService.store(document);
                    _dbContext.SaveChanges();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            else
            {
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

        if (!IsAttributeNull(updateRequest.ContentType))
        {
            document.Extension = updateRequest.ContentType;
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
        return attribute is null or "";
    }
    
    
    /**
     * Private method to check if parent category is valid.
     */
    private void CheckParentCategory(Category? parentCategory, int? parentId, string name)
    {
        // If category parent does not exist.
        if (parentId != null && parentCategory == null)
        {
            throw new Exception("Parent category with ID " + parentId + " Not found in database.");
        }
        
        // Check parent parent or root category children document.
        if (parentCategory == null)
        {
            var documents = _dbContext.Documents.Where(d => d.Category == null).ToList();
            if (documents.Any(d => d.Title.Equals(name)))
            {
                throw new Exception("Document with name '" + name + "' already exists in the parent category.");
            }
        }
        else
        {
            if (parentCategory.ChildrenDocuments.Any(d => d.Title.Equals(name)))
            {
                throw new Exception("Document with name '" + name + "' already exists in the parent category.");
            }
        }
    }
}