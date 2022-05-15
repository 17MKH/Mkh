using Mkh.Auth.Abstractions;
using Mkh.Excel.Abstractions;
using Mkh.Module.Abstractions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mkh.Excel.EPPlus;

public class EPPlusExcelHandler : ExcelHandlerAbstract
{
    public EPPlusExcelHandler(IAccount loginInfo, IExcelExportHandler exportHandler, ExcelConfig config, CommonOptions commonOptions) : base(loginInfo, exportHandler, config, commonOptions)
    {
    }
}