using ftreel.Dto.document;
using ftreel.Entities;
using Document = System.Reflection.Metadata.Document;

namespace ftreel.Dto.category;

public class CategoryDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public int? ParentCategoryId { get; set; }
    public CategoryItemDTO? ParentCategory { get; set; }

    public IList<CategoryItemDTO> ChildrenCategories { get; set; } = new List<CategoryItemDTO>();
    
    public IList<DocumentItemDTO> ChildrenDocuments { get; set; } = new List<DocumentItemDTO>();

    public CategoryDTO(int id, string name, int? parentCategoryId, CategoryItemDTO? parentCategory)
    {
        Id = id;
        Name = name;
        ParentCategoryId = parentCategoryId;
        ParentCategory = parentCategory;
    }

    public CategoryDTO(Category? category)
    {
        Id = category.Id;
        Name = category.Name;
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