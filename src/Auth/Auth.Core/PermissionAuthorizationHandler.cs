using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Mkh.Auth.Abstractions;
using Mkh.Auth.Abstractions.Annotations;
using Mkh.Auth.Abstractions.Options;

namespace Mkh.Auth.Core;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IOptionsMonitor<AuthOptions> _options;
    private readonly IPermissionValidateHandler _permissionValidateHandler;

    public PermissionAuthorizationHandler(IOptionsMonitor<AuthOptions> options, IPermissionValidateHandler permissionValidateHandler)
    {
        _options = options;
        _permissionValidateHandler = permissionValidateHandler;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        //此处判断一下是否已认证
        if (!context.User.Identity!.IsAuthenticated)
        {
            return;
        }

        var httpContext = (context.Resource as DefaultHttpContext)!.HttpContext;

        //禁用权限验证
        if (!_options.CurrentValue.EnablePermissionVerify)
        {
            context.Succeed(requirement);
            return;
        }

        //排除登录即可访问的接口
        var endpoint = httpContext.GetEndpoint();
        if (endpoint!.Metadata.Any(m => m.GetType() == typeof(AllowWhenAuthenticatedAttribute)))
        {
            context.Succeed(requirement);
            return;
        }

        var routes = httpContext.Request.RouteValues;
        var httpMethod = GetHttpMethod(httpContext.Request.Method);

        //验证权限
        if (await _permissionValidateHandler.Validate(routes, httpMethod))
        {
            context.Succeed(requirement);
        }
    }

    private HttpMethod GetHttpMethod(string method)
    {
        switch (method)
        {
            case "GET":
                return HttpMethod.Get;
            case "POST":
                return HttpMethod.Post;
            case "PUT":
                return HttpMethod.Put;
            case "DELETE":
                return HttpMethod.Delete;
            case "HEAD":
                return HttpMethod.Head;
            case "OPTIONS":
                return HttpMethod.Options;
            case "PATCH":
                return HttpMethod.Patch;
            case "TRACE":
                return HttpMethod.Trace;
            default:
                return HttpMethod.Get;
        }
    }
}