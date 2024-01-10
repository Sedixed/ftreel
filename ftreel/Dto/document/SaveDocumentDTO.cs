namespace ftreel.Dto.document;

public class SaveDocumentDTO
{
    public int? Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? CategoryId { get; set; }
    public string? Base64 { get; set; }

    public SaveDocumentDTO()
    {
    }

    public SaveDocumentDTO(int id, string title, string description, string base64)
    {
        Id = id;
        Title = title;
        Description = description;
        Base64 = base64;
    }
}