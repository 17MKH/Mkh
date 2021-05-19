using System.Collections.Generic;

namespace Mkh.Auth.Abstractions
{
    /// <summary>
    /// 权限解析器
    /// </summary>
    public interface IPermissionResolver
    {
        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <returns></returns>
        List<PermissionDescriptor> GetPermissions();
    }
}
