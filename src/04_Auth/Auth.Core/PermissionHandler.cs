using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Mkh.Auth.Abstractions.Annotations;

namespace Mkh.Auth.Core
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly ILogger _logger;

        public PermissionHandler(ILogger<PermissionHandler> logger)
        {
            _logger = logger;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var httpContext = (context.Resource as DefaultHttpContext)!.HttpContext;

            //排除登录即可访问的接口
            var endpoint = httpContext.GetEndpoint();
            if (endpoint!.Metadata.Any(m => m.GetType() == typeof(AllowWhenAuthenticatedAttribute)))
                context.Succeed(requirement);

            var routes = httpContext.Request.RouteValues;
            _logger.LogDebug("授权处理");

            //var httpMethod = (HttpMethod)Enum.Parse(typeof(HttpMethod), context.HttpContext.Request.Method);
            //var handler = context.HttpContext.RequestServices.GetService<IPermissionValidateHandler>();

            return Task.CompletedTask;
        }
    }
}
