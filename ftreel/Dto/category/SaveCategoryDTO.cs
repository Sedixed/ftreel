namespace ftreel.Dto.category;

public class SaveCategoryDTO
{
    public string? Name { get; set; } = "";

    public int? ParentCategoryId { get; set; }
    
    public SaveCategoryDTO() {
    }
    
    public SaveCategoryDTO(string name, int? parentCategoryId)
    {
        Name = name;
        ParentCategoryId = parentCategoryId;
    }
}