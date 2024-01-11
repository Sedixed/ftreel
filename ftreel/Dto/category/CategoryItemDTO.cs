using ftreel.Entities;

namespace ftreel.Dto.category;

public class CategoryItemDTO
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Path { get; set; }
    
    public bool Subscribed { get; set; } = false;
    
    public CategoryItemDTO(Category category, User? currentLoggedUser)
    {
        Id = category.Id;
        Name = category.Name;
        Path = category.GetPath();
        foreach (var user in category.Followers)
        {
            if (currentLoggedUser != null && user.Id == currentLoggedUser.Id)
            {
                Subscribed = true;
            }
        }
    }
    
    public CategoryItemDTO(Category category)
    {
        Id = category.Id;
        Name = category.Name;
        Path = category.GetPath();
    }
}