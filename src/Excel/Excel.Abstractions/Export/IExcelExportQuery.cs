using Mkh.Data.Abstractions.Entities;

namespace Mkh.Excel.Abstractions.Export;

/// <summary>
/// 为查询Dto添加Excel导出功能
/// </summary>
public interface IExcelExportQuery<TEntity> where TEntity : IEntity
{
    /// <summary>
    /// 是否是导出请求
    /// </summary>
    bool IsExport { get; set; }

    /// <summary>
    /// 导出模型
    /// </summary>
    ExcelExportEntityModel<TEntity> ExportModel { get; set; }
}