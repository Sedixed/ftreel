using ftreel.Constants;
using ftreel.Entities;

namespace ftreel.Services;

/**
 * Class that manage the storage and contents of the files.
 */
public class FileSystemStorageService
{
    
    /**
     * Store a file in upload file.
     */
    public void store(Document document)
    {
        var bytes = Convert.FromBase64String(document.Base64);

        var path = GetFilePath(document);
        
        File.WriteAllBytes(path, bytes);
    }

    /**
     * Get the file path using a document.
     */
    private static string GetFilePath(Document document)
    {
        return UploadPath.UPLOAD_FILE + document.Title + "_" + document.Id + document.Extension;
    }
}