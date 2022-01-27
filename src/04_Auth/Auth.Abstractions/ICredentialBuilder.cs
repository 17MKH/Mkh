using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mkh.Auth.Abstractions;

/// <summary>
/// 认证凭证构造器
/// </summary>
public interface ICredentialBuilder
{
    /// <summary>
    /// 生成凭证
    /// </summary>
    /// <param name="claims">账户声明</param>
    /// <returns></returns>
    Task<ICredential> Build(List<Claim> claims);
}