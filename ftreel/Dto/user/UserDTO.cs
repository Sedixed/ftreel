using ftreel.Dto.category;
using ftreel.Entities;

namespace ftreel.Dto.user;

public class UserDTO
{
    public int? Id { get; set; }
    public string? Mail { get; set; }
    public IList<string>? Roles { get; set; }
    public IList<CategoryItemDTO>? FollowedCategories { get; set; } = new List<CategoryItemDTO>();

    public UserDTO(User? user)
    {
        Id = user?.Id;
        Mail = user?.Mail;
        Roles = user?.Roles;
        foreach (var category in user?.FollowedCategories)
        {
            FollowedCategories.Add(new CategoryItemDTO(category));
        }
    }
}