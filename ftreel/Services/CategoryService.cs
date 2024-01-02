using ftreel.DATA;
using ftreel.Dto.category;
using ftreel.Entities;
using ftreel.Exceptions;

namespace ftreel.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDBContext _dbContext;

    public CategoryService(AppDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Category FindCategory(int id)
    {
        var category = _dbContext.Categories.Find(id);

        if (category == null)
        {
            throw new ObjectNotFoundException();
        }

        return category;
    }

    public IList<Category> FindAllCategories()
    {
        var categories = _dbContext.Categories.ToList();
        return categories;
    }

    /**
     * Create a category in database.
     */
    public Category CreateCategory(SaveCategoryDTO createRequest)
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
        
        _dbContext.Add(category);
        _dbContext.SaveChanges();

        return category;
    }

    public Category UpdateCategory(int id, SaveCategoryDTO updateRequest)
    {
        return null;
    }

    public void DeleteCategory(int id)
    {
        
    }
}