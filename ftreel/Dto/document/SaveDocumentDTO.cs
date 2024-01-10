namespace ftreel.Dto.document;

public class SaveDocumentDTO
{
    public int? Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ContentType { get; set; }
    public string? Author { get; set; }
    public int? CategoryId { get; set; }
    public string? Base64 { get; set; }

    public SaveDocumentDTO()
    {
    }

    public SaveDocumentDTO(int id, string title, string description, string contentType, string author, string base64)
    {
        Id = id;
        Title = title;
        Description = description;
        ContentType = contentType;
        Author = author;
        Base64 = base64;
    }
}