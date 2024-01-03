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
        
        List<Category> categories = _dbContext.Categories.Where(c => c.ParentCategory == null).ToList();

        if (pathList.Count == 0)
        {
            var category = new Category
            {
                Name = "/",
                ChildrenCategories = categories
            };
            return category;
        }

        Category? currentCategory = null;
        var i = 1;
        foreach (var name in pathList)
        {
            foreach (var category in categories)
            {
                Console.WriteLine(category.Name);
                if (category.Name.Equals(name)) {
                    currentCategory = category;
                    categories = (List<Category>) currentCategory.ChildrenCategories;
                    break;
                }
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
        var parentCategory = _dbContext.Categories.Find(createRequest.ParentCategoryId);

        // If category parent does not exist.
        if (createRequest.ParentCategoryId != null && parentCategory == null)
        {
            throw new Exception("Parent category with ID " + createRequest.ParentCategoryId + " Not found in database.");
        }
        
        // Check parent parent or root category children.
        if (parentCategory == null)
        {
            var categories = _dbContext.Categories.Where(c => c.ParentCategory == null).ToList();
            foreach (var categoryToCheck in categories)
            {
                if (categoryToCheck.Name.Equals(createRequest.Name))
                {
                    throw new Exception("Category with name '" + createRequest.Name + "' already exists in the parent category.");
                }
            }
        }
        else
        {
            foreach (var categoryToCheck in parentCategory.ChildrenCategories)
            {
                if (categoryToCheck.Name.Equals(createRequest.Name))
                {
                    throw new Exception("Category with name '" + createRequest.Name + "' already exists in the parent category.");
                }
            }
                
        }

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
        return null;
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
            Console.WriteLine("ui");
            categoryParentCategory.ChildrenCategories.Remove(category);
            category.ParentCategory = null;
            category.ParentCategoryId = null;

        }
        
        _dbContext.Categories.Remove(category);
        _dbContext.SaveChanges();
    } 
}