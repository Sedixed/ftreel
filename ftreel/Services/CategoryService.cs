using ftreel.DATA;
using ftreel.Dto.category;
using ftreel.Entities;
using ftreel.Exceptions;

namespace ftreel.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDBContext _dbContext;
    private readonly IDocumentService _documentService;

    public CategoryService(AppDBContext dbContext, IDocumentService documentService)
    {
        _dbContext = dbContext;
        _documentService = documentService;
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
    public Category? FindCategoryWithPath(string path)
    {
        IList<string> pathList = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        
        // Root categories.
        List<Category> categories = _dbContext.Categories.Where(c => c.ParentCategory == null).ToList();

        if (pathList.Count == 0)
        {
            var documents = _dbContext.Documents.Where(d => d.Category == null).ToList();
            var rootCategory = new Category
            {
                Name = "/",
                ChildrenCategories = categories,
                ChildrenDocuments = documents
            };
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

        return currentCategory;
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
    public Category? UpdateCategory(int id, SaveCategoryDTO updateRequest)
    {
        var category = _dbContext.Categories.Find(id);

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

        foreach (var childrenCategory in category.ChildrenCategories)
        {
            DeleteCategory(childrenCategory.Id);
        }
        
        foreach (var childrenDocument in category.ChildrenDocuments)
        {
            _documentService.DeleteDocument(childrenDocument.Id);
        }

        var categoryParentCategory = category.ParentCategory;
        if (categoryParentCategory != null)
        {
            categoryParentCategory.ChildrenCategories.Remove(category);
            category.ParentCategory = null;
            category.ParentCategoryId = null;

        }
        
        _dbContext.Categories.Remove(category);
        _dbContext.SaveChanges();
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