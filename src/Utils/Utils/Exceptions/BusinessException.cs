using System;

namespace Mkh.Utils.Exceptions;

/// <summary>
/// 由MKH抛出的业务异常
/// </summary>
public abstract class BusinessException : Exception
{
    public string ModuleCode { get; }

    public Enum ErrorCode { get; }

    protected BusinessException(string moduleCode, Enum errorCode, string message = null) : base(message)
    {
        ModuleCode = moduleCode;
        ErrorCode = errorCode;
    }
}