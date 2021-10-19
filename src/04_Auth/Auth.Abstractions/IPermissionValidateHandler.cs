using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mkh.Auth.Abstractions;

/// <summary>
/// 权限验证处理接口
/// </summary>
public interface IPermissionValidateHandler
{
    /// <summary>
    /// 验证
    /// </summary>
    /// <returns></returns>
    Task<bool> Validate(IDictionary<string, object> routeValues, HttpMethod httpMethod);
}