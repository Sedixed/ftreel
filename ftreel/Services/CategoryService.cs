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
                currentCategory = null;
            }
            else
            {
                throw new ObjectNotFoundException();
            }
        }

        return currentCategory;
    }

    public IList<Category?> FindAllCategories(string path)
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

        if (createRequest.ParentCategoryId != null && parentCategory == null)
        {
            throw new Exception("Parent category with ID " + createRequest.ParentCategoryId + " Not found in database.");
        }
        
        var category = new Category
        {
            Name = createRequest.Name,
            ParentCategory = parentCategory,
            ParentCategoryId = parentCategory?.Id
        };

        if (parentCategory != null)
        {
            parentCategory.ChildrenCategories.Add(category);
        }
        
        _dbContext.Add(category);
        _dbContext.SaveChanges();

        return category;
    }

    public Category? UpdateCategory(int id, SaveCategoryDTO updateRequest)
    {
        return null;
    }

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