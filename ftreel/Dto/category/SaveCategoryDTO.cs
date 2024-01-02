namespace ftreel.Dto.category;

public class SaveCategoryDTO
{
    public string Name { get; set; } = "";

    public int? ParentId { get; set; }
    
    public SaveCategoryDTO() {
    }
    
    public SaveCategoryDTO(string name, int? parentId)
    {
        Name = name;
        ParentId = parentId;
    }
}