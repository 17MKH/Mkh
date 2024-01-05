using Mkh.Utils.Exceptions;

namespace Mkh.Data.Abstractions.Exceptions;

public class EntityUpdateException : SystemException
{
    public EntityUpdateException() : base("Data", "UpdateError")
    {
    }
}