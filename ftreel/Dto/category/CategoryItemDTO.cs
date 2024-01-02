using ftreel.Entities;

namespace ftreel.Dto.category;

public class CategoryItemDTO
{
    public int Id { get; set; }

    public string Name { get; set; }
    
    public CategoryItemDTO(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public CategoryItemDTO(Category category)
    {
        Id = category.Id;
        Name = category.Name;
    }
}