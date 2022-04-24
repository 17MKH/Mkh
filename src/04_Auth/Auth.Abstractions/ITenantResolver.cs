using System;
using System.Threading.Tasks;

namespace Mkh.Auth.Abstractions;

/// <summary>
/// 租户解析器接口
/// </summary>
public interface ITenantResolver
{
    /// <summary>
    /// 解析
    /// </summary>
    /// <returns></returns>
    Task<Guid?> Resolve();
}