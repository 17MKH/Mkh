using System;

namespace Mkh.Excel.Abstractions.Annotations;

/// <summary>
/// Excel导出中忽略该属性
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class IgnoreOnExcelExportAttribute : Attribute
{
}