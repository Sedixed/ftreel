using ftreel.Dto.category;
using ftreel.Entities;

namespace ftreel.Services;

public interface ICategoryService
{
    /**
     * Find a category by its ID.
     */
    Category FindCategory(int id);

    /**
     * Find all categories.
     */
    IList<Category> FindAllCategories();

    /**
     * Create a category.
     */
    Category CreateCategory(SaveCategoryDTO createRequest);

    /**
     * Update an existing category.
     */
    Category UpdateCategory(SaveCategoryDTO updateRequest);

     /**
      * Delete a category.
      */
    void DeleteCategory(int id);
}