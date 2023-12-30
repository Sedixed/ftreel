namespace ftreel.Dto.document;

public class SaveDocumentDTO
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Base64 { get; set; } = string.Empty;

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