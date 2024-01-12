using ftreel.Constants;
using ftreel.Entities;
using ftreel.Exceptions;
using ftreel.Dto.error;

namespace ftreel.Services;

/**
 * Class that manage the storage and contents of the files from the upload file.
 */
public class FileSystemStorageService : IStorageService
{
    
    /**
     * Load the base 64 content of a file from upload directory.
     */
    public Document loadBase64(Document document)
    {
        var path = GetFilePath(document);

        if (!File.Exists(path))
        {
            throw new StorageException("File " + path + " does not exist.");
        }

        var bytes = File.ReadAllBytes(path);
        document.Base64 = Convert.ToBase64String(bytes);
        
        return document;
    }

    /**
     * Store a file in upload directory.
     */
    public void store(Document document)
    {
        var bytes = Convert.FromBase64String(document.Base64);

        var path = GetFilePath(document);
        
        File.WriteAllBytes(path, bytes);
    }

    /**
     * Delete a file from upload directory.
     */
    public void delete(Document document)
    {
        var path = GetFilePath(document);

        if (!File.Exists(path))
        {
            throw new StorageException("File " + path + " does not exist.");
        }
        
        File.Delete(path);
    }


    /**
     * Get the file path using a document.
     */
    private static string GetFilePath(Document document)
    {
        return UploadPath.UPLOAD_FILE + document.Title + "_" + document.Id;
    }
}