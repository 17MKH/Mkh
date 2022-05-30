using Mkh.Auth.Abstractions;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Text;
using Mkh.Data.Abstractions.Entities;
using Mkh.Excel.Abstractions.Export;

namespace Mkh.Excel.EPPlus;

internal class EPPlusExcelExportBuilder : IExcelExportBuilder
{
    private readonly IAccount _account;

    public EPPlusExcelExportBuilder(IAccount account)
    {
        ExcelPackage.LicenseContext = LicenseContext.Commercial;
        _account = account;
    }

    public Task Build<T>(ExcelExportEntityModel<T> model, Stream stream) where T : IEntity
    {
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add(model.Title);

        var index = new SheetRowIndex();

        SetTitle(worksheet, model, index);

        SetDescription(worksheet, model, index);

        SetColumnName(worksheet, model.ShowColumnName, model.Columns, index);

        SetColumn(worksheet, model.Columns, model.Entities, index);

        worksheet.Cells.AutoFitColumns();

        return package.SaveAsAsync(stream);
    }

    /// <summary>
    /// 设置标题
    /// </summary>
    private void SetTitle<T>(ExcelWorksheet sheet, ExcelExportEntityModel<T> model, SheetRowIndex index) where T : IEntity
    {
        if (!model.ShowTitle)
            return;

        sheet.Row(index.Next).Height = 30;
        var title = sheet.Cells[1, 1, 1, model.Columns.Count];
        title.Value = model.Title;
        title.Merge = true;
        title.Style.Font.Size = 17;
        title.Style.Font.Color.SetColor(Color.Black);
        title.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        title.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        title.Style.Fill.PatternType = ExcelFillStyle.Solid;
        title.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(221, 235, 247));

        index.Next++;
    }

    /// <summary>
    /// 设置说明
    /// </summary>
    private void SetDescription<T>(ExcelWorksheet sheet, ExcelExportEntityModel<T> model, SheetRowIndex index) where T : IEntity
    {
        var subSb = new StringBuilder();
        if (model.ShowExportPeople)
        {
            subSb.AppendFormat("导出人：{0}    ", _account.AccountName);
        }

        if (model.ShowExportDate)
        {
            subSb.AppendFormat("导出时间：{0}    ", DateTime.Now.Format());
        }

        if (model.ShowCopyright)
        {
            subSb.AppendFormat("版权所有：{0}", model.Copyright);
        }

        if (subSb.Length < 1)
            return;

        sheet.Row(index.Next).Height = 20;
        var cell = sheet.Cells[index.Next, 1, 2, model.Columns.Count];
        cell.Value = subSb.ToString();
        cell.Merge = true;
        cell.Style.Font.Size = 10;
        cell.Style.Font.Color.SetColor(Color.Black);
        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
        cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(198, 224, 180));

        index.Next++;
    }

    /// <summary>
    /// 设置列名
    /// </summary>
    private void SetColumnName(ExcelWorksheet sheet, bool showColumnName, IList<ExcelExportColumnModel> columns, SheetRowIndex index)
    {
        if (!showColumnName)
            return;

        sheet.Row(index.Next).Height = 25;
        for (int i = 0; i < columns.Count; i++)
        {
            var col = columns[i];
            var cell = sheet.Cells[index.Next, i + 1];
            cell.Value = col.Label;
            cell.Style.Font.Size = 12;
            cell.Style.Font.Bold = true;
            cell.Style.Font.Color.SetColor(Color.CornflowerBlue);
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        }

        index.Next++;
    }

    /// <summary>
    /// 设置列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sheet"></param>
    /// <param name="columns"></param>
    /// <param name="entities"></param>
    /// <param name="index"></param>
    private void SetColumn<T>(ExcelWorksheet sheet, IList<ExcelExportColumnModel> columns, IList<T> entities, SheetRowIndex index) where T : IEntity
    {
        foreach (var entity in entities)
        {
            sheet.Row(index.Next).Height = 20;

            for (int i = 0; i < columns.Count; i++)
            {
                var col = columns[i];
                var cell = sheet.Cells[index.Next, i + 1];
                cell.Style.Font.Size = 11;
                cell.Style.Font.Color.SetColor(Color.Black);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                if (col.PropertyInfo == null)
                {
                    cell.Value = "";
                }
                else
                {
                    var type = col.PropertyInfo.PropertyType;
                    if (col.PropertyInfo.PropertyType.IsNullable())
                    {
                        type = Nullable.GetUnderlyingType(type);
                    }

                    if (type.IsDateTime())
                    {
                        cell.Style.Numberformat.Format = "yyyy/MM/dd HH:mm:ss";
                        cell.Formula = "=DATE(2014,10,5)";
                        //格式化
                        if (col.Format.NotNull())
                        {
                            cell.Style.Numberformat.Format = col.Format;
                        }
                    }

                    cell.Value = col.PropertyInfo.GetValue(entity);
                }
            }

            index.Next++;
        }
    }
}

internal class SheetRowIndex
{
    public int Next { get; set; } = 1;
}