using ftreel.DATA;
using ftreel.Dto.category;
using ftreel.Entities;

namespace ftreel.Services;

public class CategoryService : ICategoryService
{
    private readonly ILogger _logger;
    private readonly AppDBContext _dbContext;

    public CategoryService(ILogger logger, AppDBContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public Category FindCategory(int id)
    {
        throw new NotImplementedException();
    }

    public IList<Category> FindAllCategories()
    {
        throw new NotImplementedException();
    }

    public Category CreateCategory(SaveCategoryDTO createRequest)
    {
        throw new NotImplementedException();
    }

    public Category UpdateCategory(SaveCategoryDTO updateRequest)
    {
        throw new NotImplementedException();
    }

    public void DeleteCategory(int id)
    {
        throw new NotImplementedException();
    }
}