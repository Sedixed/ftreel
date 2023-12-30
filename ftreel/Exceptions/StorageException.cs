using System.Runtime.Serialization;

namespace ftreel.Exceptions;

public class StorageException : Exception
{
    public StorageException()
    {
    }

    protected StorageException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public StorageException(string? message) : base(message)
    {
    }

    public StorageException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}