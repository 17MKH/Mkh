using System;

namespace Mkh.Auth.Abstractions;

/// <summary>
/// 账户信息
/// </summary>
public interface IAccount
{
    /// <summary>
    /// 租户编号
    /// </summary>
    Guid? TenantId { get; }

    /// <summary>
    /// 账户编号
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// 账户名称
    /// </summary>
    string AccountName { get; }

    /// <summary>
    /// 应用平台
    /// <para>1-99为系统保留平台类型，用户需要自定义可使用99之后的数字</para>
    /// </summary>
    int Platform { get; }

    /// <summary>
    /// 获取当前用户IP(包含IPv和IPv6)
    /// </summary>
    string IP { get; }

    /// <summary>
    /// 获取当前用户IPv4
    /// </summary>
    string IPv4 { get; }

    /// <summary>
    /// 获取当前用户IPv6
    /// </summary>
    string IPv6 { get; }

    /// <summary>
    /// 登录时间戳
    /// </summary>
    long LoginTime { get; }

    /// <summary>
    /// 获取UA
    /// </summary>
    string UserAgent { get; }
}