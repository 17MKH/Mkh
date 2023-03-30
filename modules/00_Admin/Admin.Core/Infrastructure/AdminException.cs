using System;
using Mkh.Utils.Exceptions;

namespace Mkh.Mod.Admin.Core.Infrastructure;

public class AdminException : MkhBusinessException
{
    public AdminException(Enum errorCode) : base("Admin", errorCode)
    {
    }

    public static void Throw(AdminErrorCode errorCode)
    {
        throw new AdminException(errorCode);
    }
}