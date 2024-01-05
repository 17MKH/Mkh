using System;

namespace Mkh.Utils.Exceptions;

/// <summary>
/// 由MKH抛出的系统异常
/// </summary>
public abstract class SystemException : Exception
{
    public string Module { get; }

    public string Code { get; }

    protected SystemException(string module, string code)
    {
        Module = module;
        Code = code;
    }
}