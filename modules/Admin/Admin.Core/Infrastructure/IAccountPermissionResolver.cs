using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mkh.Mod.Admin.Core.Infrastructure;

/// <summary>
/// 账户权限解析器接口
/// </summary>
public interface IAccountPermissionResolver
{
    /// <summary>
    /// 解析
    /// </summary>
    /// <param name="accountId">账户编号</param>
    /// <param name="platform">平台</param>
    /// <returns></returns>
    Task<IList<string>> Resolve(Guid accountId, int platform);
}