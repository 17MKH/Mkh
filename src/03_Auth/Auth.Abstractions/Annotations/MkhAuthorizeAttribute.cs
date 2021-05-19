using System;
using Microsoft.AspNetCore.Authorization;

namespace Mkh.Auth.Abstractions.Annotations
{
    /// <summary>
    /// Mkh授权特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class MkhAuthorizeAttribute : AuthorizeAttribute
    {
    }
}
