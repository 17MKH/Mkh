using Mkh.Excel.Abstractions;
using Mkh.Module.Abstractions.Options;
using Microsoft.Extensions.Options;
using Mkh.Excel.Abstractions.Export;
using Mkh.Excel.Core;
using Mkh.Identity.Abstractions;

namespace Mkh.Excel.EPPlus;

internal class EPPlusExcelProvider : ExcelProviderAbstract
{
    public EPPlusExcelProvider(ICurrentAccount loginInfo, IExcelExportBuilder exportBuilder, IOptionsMonitor<ExcelOptions> options, IOptionsMonitor<CommonOptions> commonOptions) : base(options, commonOptions, exportBuilder)
    {
    }
}