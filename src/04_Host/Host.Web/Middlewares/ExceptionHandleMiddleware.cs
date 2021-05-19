using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mkh.Utils.Result;

namespace Mkh.Host.Web.Middlewares
{
    public class ExceptionHandleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;
        private readonly ILogger _logger;
        public ExceptionHandleMiddleware(RequestDelegate next, IHostEnvironment env, ILogger<ExceptionHandleMiddleware> logger)
        {
            _next = next;
            _env = env;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            //开发环境返回详细异常信息
            var error = _env.IsDevelopment() ? exception.ToString() : exception.Message;

            _logger.LogError(error);

            return context.Response.WriteAsync(JsonSerializer.Serialize(ResultModel.Failed(error)));
        }
    }
}
