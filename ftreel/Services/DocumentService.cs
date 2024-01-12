using System.Security.Principal;
using System.Text.RegularExpressions;
using ftreel.DATA;
using ftreel.Dto.document;
using ftreel.Entities;
using ftreel.Exceptions;
using ftreel.Constants;
using Microsoft.OpenApi.Extensions;
using ftreel.Dto.error;

namespace ftreel.Services;

public class DocumentService : IDocumentService
{
    private readonly ILogger _logger;
    private readonly IStorageService _fileSystemStorageService;
    private readonly IMailService _mailService;
    private readonly AuthenticationService _authenticationService;
    private readonly AppDBContext _dbContext;

    public DocumentService(ILogger<DocumentService> logger, IStorageService fileSystemStorageService, IMailService mailService, AuthenticationService authenticationService, AppDBContext dbContext)
    {
        _logger = logger;
        _fileSystemStorageService = fileSystemStorageService;
        _mailService = mailService;
        _authenticationService = authenticationService;
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
    public Document SaveDocument(SaveDocumentDTO uploadRequest, IIdentity identity)
    {
        var user = _authenticationService.GetAuthenticatedUser(identity);
        
        // Create the document.
        var document = new Document(
            uploadRequest.Title,
            uploadRequest.Description,
            null,
            user,
            null,
            null
        );
        
        var match = Regex.Match(uploadRequest.Base64, @"data:(?<mime>[\w/\-]+);base64,(?<data>.+)");

        if (match.Success)
        {
            var mimeTypeRequest = match.Groups["mime"].Value;
            var base64DataRequest = match.Groups["data"].Value;

            document.ContentType = mimeTypeRequest;
            document.Base64 = base64DataRequest;
        }

        // Associate to category.
        var parentCategory = _dbContext.Categories.Find(uploadRequest.CategoryId);
        CheckParentCategory(parentCategory, parentCategory?.Id, document.Title);
        document.CategoryId = parentCategory?.Id;
        document.Category = parentCategory;

        if (user.Roles.Contains(Roles.ROLE_ADMIN.ToString()))
        {
            document.IsValidated = true;
            if (document.Category != null)
            {
                foreach (var userBis in document.Category.Followers)
                {
                    _mailService.SendMail(userBis, document);
                }
            }
        }

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
    public Document UpdateDocument(SaveDocumentDTO updateRequest)
    {
        var document = _dbContext.Documents.Find(updateRequest.Id);
        
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
                ContentType = document.ContentType
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

    /**
     * Find all documents that are not validated.
     */
    public IList<Document> GetNotValidatedDocuments()
    {
        var documents = _dbContext.Documents.Where(d => d.IsValidated == false).ToList();
        return documents;
    }

    /**
     * Validate a document.
     */
    public void ValidateDocument(int id, IIdentity identity)
    {
        var document = _dbContext.Documents.Find(id);

        if (document == null)
        {
            throw new ObjectNotFoundException();
        }

        if (!document.IsValidated)
        {
            document.IsValidated = true;
            _dbContext.SaveChanges();
        }
        else
        {
            throw new Exception("The document with ID " + document.Id + " is already validated.");
        }

        if (document.Category == null) return;
        foreach (var user in document.Category.Followers)
        {
            _mailService.SendMail(user, document);
        }
    }

    /**
     * Like a document.
     */
    public void LikeDocument(int id, IIdentity identity)
    {
        var document = _dbContext.Documents.Find(id);

        if (document == null)
        {
            throw new ObjectNotFoundException();
        }

        var user = _authenticationService.GetAuthenticatedUser(identity);
        
        user.LikedDocuments.Add(document);
        document.Likes.Add(user);

        _dbContext.SaveChanges();
    }

    /**
     * Unlike a document.
     */
    public void UnlikeDocument(int id, IIdentity identity)
    {
        var document = _dbContext.Documents.Find(id);

        if (document == null)
        {
            throw new ObjectNotFoundException();
        }
        
        var user = _authenticationService.GetAuthenticatedUser(identity);
        
        user.LikedDocuments.Remove(document);
        document.Likes.Remove(user);

        _dbContext.SaveChanges();
    }
    
    /**
     * Patch all document data provided a SaveDocumentDTO.
     */
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

        if (!IsAttributeNull(updateRequest.Base64))
        {
            var match = Regex.Match(updateRequest.Base64, @"data:(?<mime>[\w/\-]+);base64,(?<data>.+)");

            if (match.Success)
            {
                var mimeTypeRequest = match.Groups["mime"].Value;
                var base64DataRequest = match.Groups["data"].Value;

                document.ContentType = mimeTypeRequest;
                document.Base64 = base64DataRequest;
            }
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