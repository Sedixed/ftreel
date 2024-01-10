namespace ftreel.Dto.category;

public class SaveCategoryDTO
{
    public int? Id { get; set; }
    public string? Name { get; set; } = "";

    public int? ParentCategoryId { get; set; }
    
    public SaveCategoryDTO() {
    }
    
    public SaveCategoryDTO(int id, string name, int? parentCategoryId)
    {
        Id = id;
        Name = name;
        ParentCategoryId = parentCategoryId;
    }
}