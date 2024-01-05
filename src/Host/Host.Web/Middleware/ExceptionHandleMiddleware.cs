using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mkh.Utils.Exceptions;
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
        catch (Utils.Exceptions.SystemException ex)
        {
            _logger.LogError("throw system exception,the module is {module},the code is {code}", ex.Module, ex.Code);

            await HandleExceptionAsync(httpContext, ex.Message, $"System.{ex.Module}.{ex.Code}");
        }
        catch (BusinessException ex)
        {
            _logger.LogError("throw business exception,the module code is {moduleCode},the error code is {errorCode}", ex.ModuleCode, ex.ErrorCode);

            await HandleExceptionAsync(httpContext, ex.Message, $"Business.{ex.ModuleCode}.{ex.ErrorCode}");
        }
        catch (Exception ex)
        {
            //开发环境返回详细异常信息
            var error = _env.IsDevelopment() ? ex.ToString() : ex.Message;

            _logger.LogError(error);

            await HandleExceptionAsync(httpContext, "System.Error", error);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, string errorCode, string error)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.OK;

        return context.Response.WriteAsync(_jsonHelper.Serialize(ResultModel.Failed(error, errorCode)));
    }
}