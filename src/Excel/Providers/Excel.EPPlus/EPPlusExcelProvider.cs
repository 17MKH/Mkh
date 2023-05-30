using Mkh.Auth.Abstractions;
using Mkh.Excel.Abstractions;
using Mkh.Module.Abstractions.Options;
using Microsoft.Extensions.Options;
using Mkh.Excel.Abstractions.Export;
using Mkh.Excel.Core;

namespace Mkh.Excel.EPPlus;

internal class EPPlusExcelProvider : ExcelProviderAbstract
{
    public EPPlusExcelProvider(IAccount loginInfo, IExcelExportBuilder exportBuilder, IOptionsMonitor<ExcelOptions> options, IOptionsMonitor<CommonOptions> commonOptions) : base(options, commonOptions, exportBuilder)
    {
    }
}