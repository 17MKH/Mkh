using System;
using Microsoft.Extensions.DependencyInjection;
using Mkh.Excel.Abstractions;

namespace Mkh.Excel.Core;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 添加Excel功能
    /// </summary>
    /// <param name="services"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IServiceCollection AddExcel(this IServiceCollection services, Action<ExcelOptionsBuilder> action)
    {
        var options = new ExcelOptionsBuilder
        {
            Services = services
        };
        if (action != null)
        {
            action.Invoke(options);
        }

        return services;
    }
}
