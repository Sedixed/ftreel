using ftreel.Entities;

namespace ftreel.Dto.category;

public class CategoryItemDTO
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool Subscribed { get; set; } = false;
    
    public CategoryItemDTO(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public CategoryItemDTO(Category category, User? currentLoggedUser)
    {
        Id = category.Id;
        Name = category.Name;
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
    }
}