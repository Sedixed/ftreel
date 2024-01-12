using System.Security.Principal;
using ftreel.Dto.category;
using ftreel.Entities;
using ftreel.Dto.error;

namespace ftreel.Services;

public interface ICategoryService
{
    /**
     * Find a category by its ID.
     */
    Category? FindCategory(int id);

    /**
     * Find a category using its path.
     */
    Category? FindCategoryWithPath(string path, string filter, string value, IIdentity identity);
    
    /**
     * Find all categories.
     */
    IList<Category?> FindAllCategories();

    /**
     * Create a category.
     */
    Category? CreateCategory(SaveCategoryDTO createRequest);

    /**
     * Update an existing category.
     */
    Category? UpdateCategory(SaveCategoryDTO updateRequest);

     /**
      * Delete a category.
      */
    void DeleteCategory(int id);

     /**
      * Subscribe the current logged user to a category.
      */
     void SubscribeCategory(int id, IIdentity? identity);

     /**
      * Unsubscribe the current logged user from a category.
      */
     void UnsubscribeCategory(int id, IIdentity? identity);
}