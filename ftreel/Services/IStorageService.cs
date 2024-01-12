using ftreel.Entities;
using ftreel.Dto.error;

namespace ftreel.Services;

public interface IStorageService
{
    
    /**
     * Load a file using document data.
     */
    public Document loadBase64(Document document);
    
    /**
     * Store a file in the storage system.
     */
    void store(Document document);

    /**
     * Delete a file using document data.
     */
    void delete(Document document);
}