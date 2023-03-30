using System;

namespace Mkh.Utils.Exceptions;

/// <summary>
/// 由MKH系统抛出的业务异常
/// </summary>
public abstract class MkhBusinessException : Exception
{
    public string ModuleCode { get; }

    public Enum ErrorCode { get; }

    protected MkhBusinessException(string moduleCode, Enum errorCode, string message = null) : base(message)
    {
        ModuleCode = moduleCode;
        ErrorCode = errorCode;
    }
}