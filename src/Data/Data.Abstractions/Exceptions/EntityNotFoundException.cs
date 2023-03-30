using Mkh.Utils.Exceptions;

namespace Mkh.Data.Abstractions.Exceptions;

public class EntityNotFoundException : MkhSystemException
{
    public EntityNotFoundException() : base("Data", "EntityNotFound")
    {
    }
}