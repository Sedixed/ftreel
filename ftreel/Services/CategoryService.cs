using System.Security.Principal;
using ftreel.DATA;
using ftreel.Dto.category;
using ftreel.Entities;
using ftreel.Exceptions;
using Document = System.Reflection.Metadata.Document;
using ftreel.Dto.error;

namespace ftreel.Services;

public class CategoryService : ICategoryService
{
    private readonly ILogger _logger;
    private readonly AppDBContext _dbContext;
    private readonly IStorageService _storageService;
    private readonly AuthenticationService _authenticationService;

    public CategoryService(ILogger<CategoryService> logger, AppDBContext dbContext, IStorageService storageService, AuthenticationService authenticationService)
    {
        _logger = logger;
        _dbContext = dbContext;
        _storageService = storageService;
        _authenticationService = authenticationService;
    }

    /**
     * Find a category using its ID.
     */
    public Category? FindCategory(int id)
    {
        var category = _dbContext.Categories.Find(id);

        if (category == null)
        {
            throw new ObjectNotFoundException();
        }

        return category;
    }

    /**
     * Find a category using its path.
     */
    public Category? FindCategoryWithPath(string path, string filter, string value, IIdentity identity)
    {
        var user = _authenticationService.GetAuthenticatedUser(identity);
        IList<string> pathList = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        
        // Root categories.
        List<Category> categories = _dbContext.Categories.Where(c => c.ParentCategory == null).ToList();

        if (pathList.Count == 0)
        {
            var documents = _dbContext.Documents.Where(d => d.Category == null && (d.IsValidated == true || d.Author.Id == user.Id)).ToList();
            var rootCategory = new Category
            {
                Name = "/",
                ChildrenCategories = categories,
                ChildrenDocuments = documents
            };
            
            ApplyDocumentFilter(rootCategory, filter, value);
            return rootCategory;
        }

        Category? currentCategory = null;
        var i = 1;
        foreach (var name in pathList)
        {
            foreach (var category in categories.Where(category => category.Name.Equals(name)))
            {
                currentCategory = category;
                categories = (List<Category>) currentCategory.ChildrenCategories;
                break;
            }

            if (currentCategory != null)
            {
                if (i != pathList.Count) {
                    i++;
                    currentCategory = null;
                }
                else
                {
                    break;
                }
            }
            else
            {
                throw new ObjectNotFoundException();
            }
        }
        
        // Filter by document validated.
        var documentsToRemove = currentCategory?.ChildrenDocuments.Where(document => !document.IsValidated && document.Author.Id != user.Id).ToList();
        
        foreach (var document in documentsToRemove)
        {
            currentCategory.ChildrenDocuments.Remove(document);
        }
        
        ApplyDocumentFilter(currentCategory, filter, value);
        
        return currentCategory;
    }

    /**
     * Allow to filter documents from a category.
     */
    private void ApplyDocumentFilter(Category category, string filter, string value)
    {
        switch (filter)
        {
            case "title":
            {
                var documentsToRemove = category.ChildrenDocuments.Where(document => !document.Title.Contains(value)).ToList();
                foreach (var document in documentsToRemove)
                {
                    category.ChildrenDocuments.Remove(document);
                }

                break;
            }
            case "description":
            {
                var documentsToRemove = category.ChildrenDocuments.Where(document => !document.Description.Contains(value)).ToList();
                foreach (var document in documentsToRemove)
                {
                    category.ChildrenDocuments.Remove(document);
                }

                break;
            }
            case "author":
            {
                var documentsToRemove = category.ChildrenDocuments.Where(document => document.Author != null && !document.Author.Mail.Contains(value)).ToList();
                foreach (var document in documentsToRemove)
                {
                    category.ChildrenDocuments.Remove(document);
                }

                break;
            }
        }
    }

    /**
     * Find all categories.
     */
    public IList<Category?> FindAllCategories()
    {
        var categories = _dbContext.Categories.ToList();
        return categories;
    }

    /**
     * Create a category in database.
     */
    public Category? CreateCategory(SaveCategoryDTO createRequest)
    {
        if (createRequest.Name == null)
        {
            throw new Exception("Name of the category cannot be empty.");
        }

        var parentCategory = _dbContext.Categories.Find(createRequest.ParentCategoryId);
        CheckParentCategory(parentCategory, parentCategory?
            .Id, createRequest.Name);

        // Create category.
        var category = new Category
        {
            Name = createRequest.Name,
            ParentCategory = parentCategory,
            ParentCategoryId = parentCategory?.Id
        };

        parentCategory?.ChildrenCategories.Add(category);

        _dbContext.Add(category);
        _dbContext.SaveChanges();

        return category;
    }

    /**
     * Update a category in database.
     */
    public Category? UpdateCategory(SaveCategoryDTO updateRequest)
    {
        var category = _dbContext.Categories.Find(updateRequest.Id);

        if (category == null)
        {
            throw new ObjectNotFoundException();
        }
        
        if (updateRequest.Name != null && !updateRequest.Name.Equals(""))
        {
            category.Name = updateRequest.Name;
        }
        
        if (updateRequest.ParentCategoryId != null && category.ParentCategoryId != updateRequest.ParentCategoryId) {
            if (updateRequest.ParentCategoryId <= 0)
            {
                if (category.ParentCategoryId != null) {
                    CheckParentCategory(null, null, category.Name);
                    category.ParentCategory = null;
                    category.ParentCategoryId = null;
                }
            }
            else
            {
                var parentCategory = _dbContext.Categories.Find(updateRequest.ParentCategoryId);
                category.ParentCategory = parentCategory;
                category.ParentCategoryId = parentCategory?.Id;
            }
        }

        _dbContext.SaveChanges();
        return category;
    }

    /**
     * Delete a category in database.
     */
    public void DeleteCategory(int id)
    {
        var category = _dbContext.Categories.Find(id);

        if (category == null)
        {
            throw new ObjectNotFoundException();
        }

        DeleteCategoryAndChildrenRecursive(category);

        // Remove category documents.
        foreach (var document in category.ChildrenDocuments.ToList())
        {
            _dbContext.Documents.Remove(document);
            try
            {
                _storageService.delete(document);
            }
            catch (StorageException e)
            {
                _logger.LogInformation(e.Message);
            }
        }

        _dbContext.Categories.Remove(category);

        _dbContext.SaveChanges();
    }

    /**
    * Subscribe the current logged user to a category.
    */
    public void SubscribeCategory(int id, IIdentity? identity)
    {
        if (identity == null)
        {
            throw new Exception("No user authenticated.");
        }
        
        var category = _dbContext.Categories.Find(id);

        if (category == null)
        {
            throw new ObjectNotFoundException();
        }

        var user = _authenticationService.GetAuthenticatedUser(identity);
        
        category.Followers.Add(user);
        user.FollowedCategories.Add(category);

        _dbContext.SaveChanges();
    }

    /**
     * Unsubscribe the current logged user from a category.
     */
    public void UnsubscribeCategory(int id, IIdentity? identity)
    {
        if (identity == null)
        {
            throw new Exception("No user authenticated.");
        }
        
        var category = _dbContext.Categories.Find(id);

        if (category == null)
        {
            throw new ObjectNotFoundException();
        }
        
        var user = _authenticationService.GetAuthenticatedUser(identity);

        user.FollowedCategories.Remove(category);
        category.Followers.Remove(user);

        _dbContext.SaveChanges();
    }
    
    private void DeleteCategoryAndChildrenRecursive(Category category)
    {
        foreach (var childCategory in category.ChildrenCategories.ToList())
        {
            // Call recursive method to delete all children.
            DeleteCategoryAndChildrenRecursive(childCategory);
            
            foreach (var document in childCategory.ChildrenDocuments.ToList())
            {
                _dbContext.Documents.Remove(document);
                try
                {
                    _storageService.delete(document);
                }
                catch (StorageException e)
                {
                    _logger.LogInformation(e.Message);
                }
            }

            _dbContext.Categories.Remove(childCategory);
        }
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
        
        // Check parent parent or root category children.
        if (parentCategory == null)
        {
            var categories = _dbContext.Categories.Where(c => c.ParentCategory == null).ToList();
            if (categories.Any(categoryToCheck => categoryToCheck != null && categoryToCheck.Name.Equals(name)))
            {
                throw new Exception("Category with name '" + name + "' already exists in the parent category.");
            }
        }
        else
        {
            if (parentCategory.ChildrenCategories.Any(categoryToCheck => categoryToCheck.Name.Equals(name)))
            {
                throw new Exception("Category with name '" + name + "' already exists in the parent category.");
            }
        }
    }
}