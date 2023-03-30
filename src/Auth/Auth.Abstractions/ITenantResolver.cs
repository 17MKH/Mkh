using System;
using System.Threading.Tasks;

namespace Mkh.Auth.Abstractions;

/// <summary>
/// 租户解析器接口
/// </summary>
public interface ITenantResolver
{
    /// <summary>
    /// 解析租户编号
    /// </summary>
    /// <returns></returns>
    Task<Guid?> ResolveId();

    /// <summary>
    /// 获取租户名称
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<string> GetTenantName(Guid? id);
}