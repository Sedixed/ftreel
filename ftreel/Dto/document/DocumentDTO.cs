using ftreel.Dto.category;
using ftreel.Entities;

namespace ftreel.Dto.document;

public class DocumentDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Path { get; set; }
    public bool Liked { get; set; } = false;
    public int NbLikes { get; set; } = 0;
    public bool IsValidated { get; set; } = false;
    public CategoryItemDTO? Category { get; set; }
    public string Base64 { get; set; } = string.Empty;

    public DocumentDTO()
    {
    }

    public DocumentDTO(Document document, User? currentLoggedUser)
    {
        Id = document.Id;
        Title = document.Title;
        Description = document.Description;
        ContentType = document.ContentType;
        Author = document.Author?.Mail;
        Path = document.GetPath();
        foreach (var user in document.Likes)
        {
            if (currentLoggedUser != null && user.Id == currentLoggedUser.Id)
            {
                Liked = true;
            }
        }
        NbLikes = document.Likes.Count;
        if (document.Category != null)
        {
            Category = new CategoryItemDTO(document.Category);
        }
        IsValidated = document.IsValidated;

        Base64 = document.Base64;
    }
}