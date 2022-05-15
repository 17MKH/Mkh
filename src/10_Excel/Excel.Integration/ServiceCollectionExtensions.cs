using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mkh.Excel.Abstractions;
using Mkh.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mkh;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 添加Excel功能
    /// </summary>
    /// <param name="services"></param>
    /// <param name="cfg"></param>
    /// <returns></returns>
    public static IServiceCollection AddExcel(this IServiceCollection services, IConfiguration cfg)
    {
        var config = new ExcelConfig();
        var section = cfg.GetSection("Excel");
        if (section != null)
        {
            section.Bind(config);
        }
        services.AddSingleton(config);

        var assemblyHelper = new AssemblyHelper();
        var assembly = assemblyHelper.LoadByNameEndString($"Mkh.Excel.{config.Provider.ToString()}");
        if (assembly == null)
            return services;

        var handlerType = assembly.GetTypes().FirstOrDefault(m => m.Name.EndsWith("ExcelHandler"));
        if (handlerType != null)
        {
            services.AddSingleton(typeof(IExcelHandler), handlerType);
        }

        var exportHandlerType = assembly.GetTypes().FirstOrDefault(m => typeof(IExcelExportHandler).IsAssignableFrom(m));
        if (handlerType != null)
        {
            services.AddSingleton(typeof(IExcelExportHandler), exportHandlerType!);
        }
        return services;
    }
}