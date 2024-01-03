using ftreel.Dto.category;
using ftreel.Entities;

namespace ftreel.Dto.document;

public class DocumentDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public CategoryItemDTO? Category { get; set; }
    public string Base64 { get; set; } = string.Empty;

    public DocumentDTO()
    {
    }

    public DocumentDTO(Document document)
    {
        Id = document.Id;
        Title = document.Title;
        Description = document.Description;
        Extension = document.Extension;
        Author = document.Author;
        if (document.Category != null)
        {
            Category = new CategoryItemDTO(document.Category);
        }

        Base64 = document.Base64;
    }
}