using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

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

    public string GetDocumentUrl()
    {
        // Implement logic to get the URL based on the hierarchical path
        var hierarchicalPath = GetPath();
        var baseUrl = "http://localhost:5173/files";

        // Split the hierarchical path by '/'
        var pathSegments = hierarchicalPath.Split('/');

        // Remove the last segment from the path
        var pathWithoutLastSegment = string.Join("/", pathSegments.Take(pathSegments.Length - 1));
        return $"{baseUrl}?path={HttpUtility.UrlEncode(pathWithoutLastSegment)}";
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