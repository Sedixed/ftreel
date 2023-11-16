public class Document
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;

    public Document()
    {
    }

    public Document(int id, string title, string description, string filePath, string extension, string author, string category)
    {
        Id = id;
        Title = title;
        Description = description;
        FilePath = filePath;
        Extension = extension;
        Author = author;
        Category = category;
    }

    public override string ToString()
    {
        return $"Id: {Id}, Title: {Title}, Description: {Description}, FilePath: {FilePath}, Extension: {Extension}, Author: {Author}, Category: {Category}";
    }
}