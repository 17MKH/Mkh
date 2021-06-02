using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mkh.Auth.Abstractions;
using Mkh.Auth.Abstractions.Annotations;
using Mkh.Auth.Abstractions.Options;
using Mkh.Utils.Web;

namespace Mkh.Auth.Core
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly ILogger _logger;
        private readonly IOptionsMonitor<AuthOptions> _options;
        private readonly IPResolver _ipResolver;

        public PermissionHandler(ILogger<PermissionHandler> logger, IOptionsMonitor<AuthOptions> options, IPResolver ipResolver)
        {
            _logger = logger;
            _options = options;
            _ipResolver = ipResolver;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            //此处判断一下是否已认证
            if (!context.User.Identity!.IsAuthenticated)
            {
                return Task.CompletedTask;
            }

            //检测IP地址
            if (_options.CurrentValue.EnableCheckIP && _ipResolver.IP != context.User.Claims.First(m => m.Type.Equals(MkhClaimTypes.IP)).Value)
            {
                return Task.CompletedTask;
            }

            var httpContext = (context.Resource as DefaultHttpContext)!.HttpContext;

            //禁用权限验证
            if (!_options.CurrentValue.EnablePermissionVerify)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            //排除登录即可访问的接口
            var endpoint = httpContext.GetEndpoint();
            if (endpoint!.Metadata.Any(m => m.GetType() == typeof(AllowWhenAuthenticatedAttribute)))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var routes = httpContext.Request.RouteValues;


            //var httpMethod = (HttpMethod)Enum.Parse(typeof(HttpMethod), context.HttpContext.Request.Method);
            //var handler = context.HttpContext.RequestServices.GetService<IPermissionValidateHandler>();

            return Task.CompletedTask;
        }
    }
}
