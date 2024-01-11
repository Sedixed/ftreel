using System.ComponentModel.DataAnnotations.Schema;

namespace ftreel.Entities;

public class Document
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsValidated { get; set; } = false;
    public string ContentType { get; set; } = string.Empty;
    public int? AuthorId { get; set; }
    public virtual User? Author { get; set; }

    public virtual IList<User> Likes { get; set; } = new List<User>();
        
    public int? CategoryId { get; set; }
    public virtual Category? Category { get; set; }
    
    [NotMapped]
    public string Base64 { get; set; } = string.Empty;

    public Document()
    {
    }

    public Document(string title, string description, string contentType, User author, Category? category, string base64)
    {
        Title = title;
        Description = description;
        ContentType = contentType;
        AuthorId = author.Id;
        Author = author;
        CategoryId = category?.Id;
        Base64 = base64;
    }

    public string GetPath()
    {
        if (Category == null)
        {
            return "/" + Title;
        }
        return GetPathRecursive(Category) + "/" + Title;
    }

    private string GetPathRecursive(Category category)
    {
        if (category.ParentCategory == null)
        {
            return "/" + category.Name;
        }

        return GetPathRecursive(category.ParentCategory) + "/" + category.Name;
    }
}