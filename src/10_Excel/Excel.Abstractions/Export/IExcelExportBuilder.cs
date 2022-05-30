using System.IO;
using System.Threading.Tasks;
using Mkh.Data.Abstractions.Entities;

namespace Mkh.Excel.Abstractions.Export;

/// <summary>
/// Excel导出提供器接口
/// </summary>
public interface IExcelExportBuilder
{
    /// <summary>
    /// 生成导出的Excel文件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model"></param>
    /// <param name="stream">保存的流</param>
    /// <returns></returns>
    Task Build<T>(ExcelExportEntityModel<T> model, Stream stream) where T : IEntity;
}
