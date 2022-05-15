using Mkh.Data.Abstractions.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mkh.Excel.Abstractions;

/// <summary>
/// 查询对象，含导出
/// </summary>
public abstract class QueryWithExportDto : QueryDto
{
    /// <summary>
    /// excel 导出配置
    /// </summary>
    public ExportModel? Export { get; set; }
}
