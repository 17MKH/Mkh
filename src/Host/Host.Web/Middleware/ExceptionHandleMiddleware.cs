using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mkh.Utils.Json;

namespace Mkh.Host.Web.Middleware;

public class ExceptionHandleMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _env;
    private readonly ILogger _logger;
    private readonly JsonHelper _jsonHelper;

    public ExceptionHandleMiddleware(RequestDelegate next, IHostEnvironment env, ILogger<ExceptionHandleMiddleware> logger, JsonHelper jsonHelper)
    {
        _next = next;
        _env = env;
        _logger = logger;
        _jsonHelper = jsonHelper;
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
        context.Response.StatusCode = (int)HttpStatusCode.OK;

        //开发环境返回详细异常信息
        var error = _env.IsDevelopment() ? exception.ToString() : exception.Message;

        _logger.LogError(error);

        return context.Response.WriteAsync(_jsonHelper.Serialize(ResultModel.Failed(error)));
    }
}