using System.Runtime.Serialization;

namespace ftreel.Exceptions;

public class ObjectNotFoundException : Exception
{
    public ObjectNotFoundException()
    {
    }

    protected ObjectNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ObjectNotFoundException(string? message) : base(message)
    {
    }

    public ObjectNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}