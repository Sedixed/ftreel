using ftreel.Dto.category;
using ftreel.Entities;

namespace ftreel.Dto.user;

public class FollowedCategoriesDTO
{
    public IList<CategoryItemDTO>? FollowedCategories { get; set; } = new List<CategoryItemDTO>();

    public FollowedCategoriesDTO(User user)
    {
        foreach (var category in user?.FollowedCategories)
        {
            FollowedCategories.Add(new CategoryItemDTO(category));
        }
    }
}