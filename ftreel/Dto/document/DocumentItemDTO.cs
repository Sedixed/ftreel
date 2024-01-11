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

    public DocumentItemDTO(Document document)
    {
        Id = document.Id;
        Title = document.Title;
        Description = document.Description;
        Extension = document.ContentType;
        Author = document.Author?.Mail;
        Path = document.GetPath();
    }
}