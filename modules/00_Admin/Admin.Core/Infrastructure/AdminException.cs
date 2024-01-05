using Mkh.Utils.Exceptions;

namespace Mkh.Mod.Admin.Core.Infrastructure;

public class AdminException : BusinessException
{
    public AdminException(AdminErrorCode errorCode) : base("Admin", errorCode)
    {
    }

    public static void Throw(AdminErrorCode errorCode)
    {
        throw new AdminException(errorCode);
    }
}