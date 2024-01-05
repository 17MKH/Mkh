using Mkh.Utils.Exceptions;

namespace Mkh.Data.Abstractions.Exceptions;

public class EntityAddException : SystemException
{
    public EntityAddException() : base("Data", "AddError")
    {
    }
}