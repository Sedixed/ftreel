using System.ComponentModel.DataAnnotations.Schema;

namespace ftreel.Entities;

public class Category
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }

    public IList<Category> ChildrenCategories { get; set; } = new List<Category>();
    
    public IList<Document> ChildrenDocuments { get; set; } = new List<Document>();

    public Category()
    {
    }

    public Category(string name, Category? parentCategory, IList<Category> childrenCategories, IList<Document> childrenDocuments)
    {
        Name = name;
        ParentCategoryId = parentCategory?.Id;
        ParentCategory = parentCategory;
        ChildrenCategories = childrenCategories;
        ChildrenDocuments = childrenDocuments;
    }
}