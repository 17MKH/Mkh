using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json.Serialization;
using Mkh.Data.Abstractions.Entities;

namespace Mkh.Excel.Abstractions.Export;

/// <summary>
/// 导出实体模型
/// </summary>
public class ExcelExportEntityModel<TEntity> where TEntity : IEntity
{
    /// <summary>
    /// 是否显示标题
    /// </summary>
    public bool ShowTitle { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 导出格式
    /// </summary>
    public ExportFormat Format { get; set; }

    /// <summary>
    /// 导出模式
    /// </summary>
    public ExportMode Mode { get; set; }

    /// <summary>
    /// 是否显示版权信息
    /// </summary>
    public bool ShowCopyright { get; set; }

    /// <summary>
    /// 版权信息
    /// </summary>
    public string Copyright { get; set; }

    /// <summary>
    /// 是否显示列名称
    /// </summary>
    public bool ShowColumnName { get; set; }

    /// <summary>
    /// 是否 显示导出日期
    /// </summary>
    public bool ShowExportDate { get; set; }

    /// <summary>
    /// 是否显示导出人
    /// </summary>
    public bool ShowExportPeople { get; set; }

    /// <summary>
    /// 导出的列信息
    /// </summary>
    public IList<ExcelExportColumnModel> Columns { get; set; }

    /// <summary>
    /// 实体集合
    /// </summary>
    [JsonIgnore]
    public IList<TEntity> Entities { get; set; }
}

/// <summary>
/// Excel导出列模型
/// </summary>
public class ExcelExportColumnModel
{
    /// <summary>
    /// 属性
    /// </summary>
    public string Prop { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// 对齐方式
    /// </summary>
    public ColumnAlign Align { get; set; }

    /// <summary>
    /// 格式化，暂时仅支持日期类型
    /// </summary>
    public string Format { get; set; }

    /// <summary>
    /// 列类型
    /// </summary>
    [JsonIgnore]
    public PropertyInfo PropertyInfo { get; set; }
}

/// <summary>
/// 列对齐方式
/// </summary>
public enum ColumnAlign
{
    [Description("Left")]
    Left,
    [Description("Center")]
    Center,
    [Description("Right")]
    Right
}

/// <summary>
/// 导出格式
/// </summary>
public enum ExportFormat
{
    [Description(".xlsx")]
    Xlsx
}

/// <summary>
/// 导出模式
/// </summary>
public enum ExportMode
{
    [Description("全部")]
    All,
    [Description("当前页")]
    CurrentPage
}
