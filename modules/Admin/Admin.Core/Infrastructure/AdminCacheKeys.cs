using System;
using Mkh.Utils.Annotations;

namespace Mkh.Mod.Admin.Core.Infrastructure;

/// <summary>
/// 权限管理模块缓存键
/// </summary>
[SingletonInject]
public class AdminCacheKeys
{
    /// <summary>
    /// 验证码
    /// </summary>
    /// <param name="id">验证码ID</param>
    /// <returns></returns>
    public string VerifyCode(string id) => $"ADMIN:VERIFY_CODE:{id}";

    /// <summary>
    /// 刷新令牌
    /// </summary>
    /// <param name="refreshToken">刷新令牌</param>
    /// <param name="platform">平台</param>
    /// <returns></returns>
    public string RefreshToken(string refreshToken, int platform) => $"ADMIN:REFRESH_TOKEN:{platform}";

    /// <summary>
    /// 账户权限列表
    /// </summary>
    /// <param name="accountId">账户编号</param>
    /// <param name="platform">平台</param>
    /// <returns></returns>
    public string AccountPermissions(Guid accountId, int platform) => $"ADMIN:ACCOUNT:PERMISSIONS:{accountId}:{platform}";

    /// <summary>
    /// 字典下拉列表
    /// </summary>
    /// <param name="groupCode">字典分组编码</param>
    /// <param name="dictCode">字典编码</param>
    /// <returns></returns>
    public string DictSelect(string groupCode, string dictCode) => $"ADMIN:DICT:SELECT:{groupCode.ToUpper()}:{dictCode.ToUpper()}";

    /// <summary>
    /// 字典树
    /// </summary>
    /// <param name="groupCode"></param>
    /// <param name="dictCode"></param>
    /// <returns></returns>
    public string DictTree(string groupCode, string dictCode) => $"ADMIN:DICT:TREE:{groupCode.ToUpper()}:{dictCode.ToUpper()}";

    /// <summary>
    /// 字典级联列表
    /// </summary>
    /// <param name="groupCode"></param>
    /// <param name="dictCode"></param>
    /// <returns></returns>
    public string DictCascader(string groupCode, string dictCode) => $"ADMIN:DICT:CASCADER:{groupCode.ToUpper()}:{dictCode.ToUpper()}";
}