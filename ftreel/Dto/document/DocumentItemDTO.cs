using ftreel.Entities;

namespace ftreel.Dto.document;

public class DocumentItemDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Extension { get; set; }
    public string Author { get; set; }
    public string Path { get; set; }
    public bool Liked { get; set; } = false;
    public int NbLikes { get; set; } = 0;
    public bool IsValidated { get; set; } = false;

    public DocumentItemDTO(Document document, User currentLoggedUser)
    {
        Id = document.Id;
        Title = document.Title;
        Description = document.Description;
        Extension = document.ContentType;
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
        IsValidated = document.IsValidated;
    }
    
    public DocumentItemDTO(Document document)
    {
        Id = document.Id;
        Title = document.Title;
        Description = document.Description;
        Extension = document.ContentType;
        Author = document.Author?.Mail;
        Path = document.GetPath();
        NbLikes = document.Likes.Count;
        IsValidated = document.IsValidated;
    }
}