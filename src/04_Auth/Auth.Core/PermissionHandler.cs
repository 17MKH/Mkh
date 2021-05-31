using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mkh.Auth.Abstractions.Annotations;
using Mkh.Auth.Abstractions.Options;

namespace Mkh.Auth.Core
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly ILogger _logger;
        private readonly IOptionsMonitor<AuthOptions> _options;

        public PermissionHandler(ILogger<PermissionHandler> logger, IOptionsMonitor<AuthOptions> options)
        {
            _logger = logger;
            _options = options;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
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
