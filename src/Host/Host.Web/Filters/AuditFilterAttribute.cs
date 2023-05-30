using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mkh.Auth.Abstractions;
using Mkh.Auth.Abstractions.Annotations;
using Mkh.Auth.Abstractions.Options;
using Mkh.Logging.Abstractions.Providers;
using Mkh.Module.Abstractions;
using Mkh.Utils.Json;

namespace Mkh.Host.Web.Filters
{
    /// <summary>
    /// 审计日志
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    internal class AuditFilterAttribute : AuthorizeAttribute, IAsyncActionFilter
    {
        private readonly IOptionsMonitor<AuthOptions> _authOptions;
        private readonly IAuditLogHandler _logHandler;
        private readonly IAccount _account;
        private readonly JsonHelper _jsonHelper;
        private readonly IModuleCollection _moduleCollection;
        private readonly ILogger<AuditFilterAttribute> _logger;

        public AuditFilterAttribute(IAuditLogHandler logHandler, IOptionsMonitor<AuthOptions> authOptions, IAccount account, JsonHelper jsonHelper, IModuleCollection moduleCollection, ILogger<AuditFilterAttribute> logger)
        {
            _logHandler = logHandler;
            _authOptions = authOptions;
            _account = account;
            _jsonHelper = jsonHelper;
            _moduleCollection = moduleCollection;
            _logger = logger;
        }


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //未启用审计日志
            if (!_authOptions.CurrentValue.EnableAuditLog)
            {
                await next();
                return;
            }
            //如果禁用审计功能，直接走下一步
            if (CheckDisabled(context))
            {
                await next();
                return;
            }

            var log = CreateLog(context);

            var sw = new Stopwatch();
            sw.Start();

            var resultContext = await next();

            sw.Stop();

            if (log != null)
            {
                try
                {
                    //执行结果
                    if (resultContext.Result is ObjectResult result)
                    {
                        log.Result = _jsonHelper.Serialize(result.Value);
                    }

                    //用时
                    log.ExecutionDuration = sw.ElapsedMilliseconds;

                    await _logHandler.Write(log);
                }
                catch (Exception ex)
                {
                    _logger.LogError("审计日志插入异常：{@ex}", ex);
                }
            }
        }


        /// <summary>
        /// 判断是否禁用审计功能
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool CheckDisabled(ActionExecutingContext context)
        {
            return context.ActionDescriptor.EndpointMetadata.Any(m => m.GetType() == typeof(DisableAuditAttribute));
        }

        private AuditLogModel CreateLog(ActionExecutingContext context)
        {
            try
            {
                var routeValues = context.ActionDescriptor.RouteValues;
                var model = new AuditLogModel
                {
                    AccountId = _account.Id,
                    TenantId = _account.TenantId,
                    AccountName = _account.AccountName,
                    ModuleCode = routeValues["area"] ?? "",
                    Controller = routeValues["controller"],
                    Action = routeValues["action"],
                    Parameters = _jsonHelper.Serialize(context.ActionArguments),
                    Platform = _account.Platform,
                    IP = _account.IP,
                    ExecutionTime = DateTime.Now,
                    UserAgent = _account.UserAgent
                };

                //获取模块的名称
                if (model.ModuleCode.NotNull())
                {
                    model.ModuleName = _moduleCollection.FirstOrDefault(m => m.Code.EqualsIgnoreCase(model.ModuleCode))?.Name;
                }

                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError("Audit Log Create Exception：{@ex}", ex);
            }

            return null;
        }
    }
}
