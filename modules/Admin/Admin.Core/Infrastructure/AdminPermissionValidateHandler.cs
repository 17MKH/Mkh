using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mkh.Auth.Abstractions;
using Mkh.Utils.Annotations;

namespace Mkh.Mod.Admin.Core.Infrastructure;

/// <summary>
/// 默认权限验证处理器
/// </summary>
[ScopedInject]
internal class AdminPermissionValidateHandler : IPermissionValidateHandler
{
    private readonly IAccountPermissionResolver _accountPermissionResolver;
    private readonly IAccount _account;

    public AdminPermissionValidateHandler(IAccountPermissionResolver accountPermissionResolver, IAccount account)
    {
        _accountPermissionResolver = accountPermissionResolver;
        _account = account;
    }

    public async Task<bool> Validate(IDictionary<string, object> routeValues, HttpMethod httpMethod)
    {
        var permissions = await _accountPermissionResolver.Resolve(_account.Id, _account.Platform);

        var area = routeValues["area"];
        var controller = routeValues["controller"];
        var action = routeValues["action"];
        return permissions.Any(m => m.EqualsIgnoreCase($"{area}_{controller}_{action}_{httpMethod}"));
    }
}