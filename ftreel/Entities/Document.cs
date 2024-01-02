using System.ComponentModel.DataAnnotations.Schema;

namespace ftreel.Entities;

public class Document
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int? CategoryId { get; set; }
    public virtual Category? Category { get; set; }
    
    [NotMapped]
    public string Base64 { get; set; } = string.Empty;

    public Document()
    {
    }

    public Document(string title, string description, string extension, string author, Category? category, string base64)
    {
        Title = title;
        Description = description;
        Extension = extension;
        Author = author;
        CategoryId = category?.Id;
        Base64 = base64;
    }
}