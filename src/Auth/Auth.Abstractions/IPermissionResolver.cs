using System.Collections.Generic;
using Mkh.Utils.Permissions;

namespace Mkh.Auth.Abstractions;

/// <summary>
/// 权限解析器
/// </summary>
public interface IPermissionResolver
{
    /// <summary>
    /// 获取指定模块的权限列表
    /// </summary>
    /// <param name="moduleCode">模块编码</param>
    /// <returns></returns>
    List<PermissionDescriptor> GetModulePermissions(string moduleCode);
}