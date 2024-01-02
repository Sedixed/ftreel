using System.ComponentModel.DataAnnotations.Schema;

namespace ftreel.Entities;

public class Category
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int? ParentCategoryId { get; set; }
    public virtual Category? ParentCategory { get; set; }

    public virtual IList<Category> ChildrenCategories { get; set; } = new List<Category>();
    
    public virtual IList<Document> ChildrenDocuments { get; set; } = new List<Document>();

    public Category()
    {
    }

    public Category(string name, Category? parentCategory)
    {
        Name = name;
        ParentCategoryId = parentCategory?.Id;
    }
}