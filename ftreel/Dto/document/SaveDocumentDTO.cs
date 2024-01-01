namespace ftreel.Dto.document;

public class SaveDocumentDTO
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? FilePath { get; set; }
    public string? Extension { get; set; }
    public string? Author { get; set; }
    public string? Category { get; set; }
    public string? Base64 { get; set; }

    public SaveDocumentDTO()
    {
    }

    public SaveDocumentDTO(string title, string description, string filePath, string extension, string author, string category, string base64)
    {
        Title = title;
        Description = description;
        FilePath = filePath;
        Extension = extension;
        Author = author;
        Category = category;
        Base64 = base64;
    }
}