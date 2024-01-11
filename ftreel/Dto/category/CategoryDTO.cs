using ftreel.Dto.document;
using ftreel.Entities;
using Document = System.Reflection.Metadata.Document;

namespace ftreel.Dto.category;

public class CategoryDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = "";
    public string Path { get; set; }
    public bool Subscribed { get; set; } = false;
    public int? ParentCategoryId { get; set; }
    public CategoryItemDTO? ParentCategory { get; set; }

    public IList<CategoryItemDTO> ChildrenCategories { get; set; } = new List<CategoryItemDTO>();
    
    public IList<DocumentItemDTO> ChildrenDocuments { get; set; } = new List<DocumentItemDTO>();

    public CategoryDTO(Category? category, User? currentLoggedUser)
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
        
        ParentCategoryId = category.ParentCategoryId;
        if (category.ParentCategory != null) ParentCategory = new CategoryItemDTO(category.ParentCategory);
        
        foreach (var childCategory in category.ChildrenCategories)
        {
            ChildrenCategories.Add(new CategoryItemDTO(childCategory, currentLoggedUser));
        }

        foreach (var childDocument in category.ChildrenDocuments)
        {
            ChildrenDocuments.Add(new DocumentItemDTO(childDocument, currentLoggedUser));
        }
    }
    
    public CategoryDTO(Category? category)
    {
        Id = category.Id;
        Name = category.Name;
        Path = category.GetPath();
        ParentCategoryId = category.ParentCategoryId;
        if (category.ParentCategory != null) ParentCategory = new CategoryItemDTO(category.ParentCategory);
        
        foreach (var childCategory in category.ChildrenCategories)
        {
            ChildrenCategories.Add(new CategoryItemDTO(childCategory));
        }

        foreach (var childDocument in category.ChildrenDocuments)
        {
            ChildrenDocuments.Add(new DocumentItemDTO(childDocument));
        }
    }
}