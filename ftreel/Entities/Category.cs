using System.ComponentModel.DataAnnotations.Schema;

namespace ftreel.Entities;

public class Category
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public int? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }

    public IList<Category> ChildrenCategories { get; set; } = new List<Category>();
    
    public IList<Document> ChildrenDocuments { get; set; } = new List<Document>();

    public Category()
    {
    }

    public Category(int id, string name, Category? parentCategory, IList<Category> childrenCategories, IList<Document> childrenDocuments)
    {
        Id = id;
        Name = name;
        ParentCategoryId = parentCategory?.Id;
        ParentCategory = parentCategory;
        ChildrenCategories = childrenCategories;
        ChildrenDocuments = childrenDocuments;
    }
}