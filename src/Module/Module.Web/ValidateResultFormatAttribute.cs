using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Mkh.Module.Web;

/// <summary>
/// 模型验证结果格式化过滤器
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class ValidateResultFormatAttribute : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            try
            {
                var errors = context.ModelState
                    .Where(m => m.Value!.ValidationState == ModelValidationState.Invalid)
                    .Select(m =>
                    {
                        var sb = new StringBuilder();
                        sb.AppendFormat("{0}：", m.Key);
                        sb.Append(m.Value.Errors.Select(n => n.ErrorMessage).Aggregate((x, y) => x + ";" + y));
                        return sb.ToString();
                    })
                    .Aggregate((x, y) => x + "|" + y);

                context.Result = new JsonResult(ResultModel.Failed(errors));
            }
            catch
            {
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}