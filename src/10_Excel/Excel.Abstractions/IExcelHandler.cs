using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mkh.Excel.Abstractions;

/// <summary>
/// Excel处理接口
/// </summary>
public interface IExcelHandler
{
    /// <summary>
    /// 导出
    /// </summary>
    /// <typeparam name="T"><![CDATA[ 实体类型，支持class、struct、DynamicObject、JObject、JsonObject或实现IDictionary<,>的对象 ]]></typeparam>
    /// <param name="model"></param>
    /// <param name="entities"></param>
    /// <returns></returns>
    ExcelExportResultModel Export<T>(ExportModel model, IList<T> entities); //where T : class, new();
}