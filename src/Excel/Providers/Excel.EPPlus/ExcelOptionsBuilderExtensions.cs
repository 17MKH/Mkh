using Microsoft.Extensions.DependencyInjection;
using Mkh.Excel.Abstractions;
using Mkh.Excel.Abstractions.Export;

namespace Mkh.Excel.EPPlus
{
    public static class ExcelOptionsBuilderExtensions
    {
        /// <summary>
        /// 使用EPPlus
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ExcelOptionsBuilder UseEPPlus(this ExcelOptionsBuilder builder)
        {
            builder.Services.AddSingleton<IExcelExportBuilder, EPPlusExcelExportBuilder>();
            builder.Services.AddSingleton<IExcelProvider, EPPlusExcelProvider>();

            return builder;
        }
    }
}
