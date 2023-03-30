using Mkh.Utils.Exceptions;

namespace Mkh.Data.Abstractions.Exceptions;

public class EntityUpdateException : MkhSystemException
{
    public EntityUpdateException() : base("Data", "UpdateError")
    {
    }
}