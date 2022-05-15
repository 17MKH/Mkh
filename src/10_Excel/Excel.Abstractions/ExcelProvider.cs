using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mkh.Excel.Abstractions;

/// <summary>
/// Excel操作库
/// </summary>
public enum ExcelProvider
{
    [Description("EPPlus")]
    EPPlus,
    [Description("NPOI")]
    NPOI,
    [Description("OpenXml")]
    OpenXml
}
