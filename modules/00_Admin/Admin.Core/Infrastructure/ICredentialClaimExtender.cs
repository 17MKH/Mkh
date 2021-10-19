using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mkh.Mod.Admin.Core.Infrastructure;

/// <summary>
/// 认证凭证中的声明扩展器，当用户需要添加自定义的声明时，可以实现该接口并注入
/// </summary>
public interface ICredentialClaimExtender
{
    /// <summary>
    /// 扩展
    /// </summary>
    /// <param name="claims">声明集合</param>
    /// <param name="accountId">账户编号</param>
    /// <returns></returns>
    Task Extend(List<Claim> claims, Guid accountId);
}