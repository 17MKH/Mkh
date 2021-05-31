using System.Collections.Generic;
using System.Security.Claims;
using Mkh.Utils.Models;

namespace Mkh.Auth.Abstractions
{
    /// <summary>
    /// 认证凭证构造器
    /// </summary>
    public interface ICredentialBuilder
    {
        /// <summary>
        /// 生成凭证
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        IResultModel Build(List<Claim> claims);
    }
}
