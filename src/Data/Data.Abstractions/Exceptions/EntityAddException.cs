using Mkh.Utils.Exceptions;

namespace Mkh.Data.Abstractions.Exceptions;

public class EntityAddException : MkhSystemException
{
    public EntityAddException() : base("Data", "AddError")
    {
    }
}