using System;

namespace Mkh.Utils.Exceptions;

/// <summary>
/// 由MKH系统抛出的系统异常
/// </summary>
public abstract class MkhSystemException : Exception
{
    public string Module { get; }

    public string Code { get; }

    protected MkhSystemException(string module, string code)
    {
        Module = module;
        Code = code;
    }
}