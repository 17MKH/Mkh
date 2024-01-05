using Mkh.Utils.Exceptions;

namespace Mkh.Data.Abstractions.Exceptions;

public class EntityNotFoundException : SystemException
{
    public EntityNotFoundException() : base("Data", "EntityNotFound")
    {
    }
}