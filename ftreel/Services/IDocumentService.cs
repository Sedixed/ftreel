using System.Security.Principal;
using ftreel.Dto.document;
using ftreel.Entities;
using ftreel.Dto.error;

namespace ftreel.Services;

public interface IDocumentService
{
    /**
     * Find a file using its ID.
     */
    Document FindDocument(int id);

    /**
     * Find all file from database and storage system.
     */
    IList<Document> FindAllDocuments();
    
    /**
     * Create a file in database and in storage system.
     */
    Document SaveDocument(SaveDocumentDTO createRequest, IIdentity identity);

    /**
     * Update a file in database and in storage system.
     */
    Document UpdateDocument(SaveDocumentDTO updateRequest);

    /**
     * Delete a file from database and storage system using its ID.
     */
    void DeleteDocument(int id);

    /**
     * Find all documents that are not validated.
     */
    IList<Document> GetNotValidatedDocuments();

    /**
     * Validate a document.
     */
    void ValidateDocument(int id, IIdentity identity);

    /**
     * Like a document.
     */
    void LikeDocument(int id, IIdentity identity);

    /**
     * Unlike a document.
     */
    void UnlikeDocument(int id, IIdentity identity);
}