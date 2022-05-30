using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Mkh.Auth.Abstractions;
using Mkh.Data.Abstractions.Entities;
using Mkh.Excel.Abstractions;
using Mkh.Excel.Abstractions.Annotations;
using Mkh.Excel.Abstractions.Export;
using Mkh.Module.Abstractions.Options;

namespace Mkh.Excel.Core;

public abstract class ExcelProviderAbstract : IExcelProvider
{
    private readonly IExcelExportBuilder _exportBuilder;
    private readonly IOptionsMonitor<ExcelOptions> _options;
    private readonly IOptionsMonitor<CommonOptions> _commonOptions;

    //导出Excel的对象的属性类型列表
    private readonly ConcurrentDictionary<RuntimeTypeHandle, Dictionary<string, PropertyInfo>> _exportObjectProperties = new();

    protected ExcelProviderAbstract(IOptionsMonitor<ExcelOptions> options, IOptionsMonitor<CommonOptions> commonOptions, IExcelExportBuilder exportBuilder)
    {
        _options = options;
        _commonOptions = commonOptions;
        _exportBuilder = exportBuilder;
    }

    /// <summary>
    /// 设置列属性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model"></param>
    private void SetColumnPropertyType<T>(ExcelExportEntityModel<T> model) where T : IEntity
    {
        if (model.Columns == null || !model.Columns.Any())
            return;

        var objectType = typeof(T);

        if (!_exportObjectProperties.TryGetValue(objectType.TypeHandle, out Dictionary<string, PropertyInfo> types))
        {
            types = new Dictionary<string, PropertyInfo>();
            var properties = objectType.GetProperties();
            foreach (var property in properties)
            {
                if (property.GetCustomAttributes().All(m => m.GetType() != typeof(IgnoreOnExcelExportAttribute)))
                {
                    types.Add(property.Name.ToLower(), property);
                }
            }

            _exportObjectProperties.TryAdd(objectType.TypeHandle, types);
        }

        foreach (var column in model.Columns)
        {
            column.PropertyInfo = types.FirstOrDefault(m => m.Key.EqualsIgnoreCase(column.Prop)).Value;
        }
    }

    public async Task<IResultModel<ExcelModel>> Export<T>(ExcelExportEntityModel<T> model) where T : IEntity
    {
        if (model == null)
            throw new NullReferenceException("Excel导出信息不存在");

        var result = new ResultModel<ExcelModel>();

        SetColumnPropertyType(model);

        var tempPath = GetTempDirectory();

        var storageName = Guid.NewGuid() + model.Format.ToDescription();

        var storageDir = Path.Combine(tempPath, "Export", DateTime.Now.Format("yyyyMMdd"));

        if (!Directory.Exists(storageDir))
        {
            Directory.CreateDirectory(storageDir);
        }

        var excelModel = new ExcelModel
        {
            FileName = model.FileName,
            StorageName = storageName,
            StoragePath = Path.Combine(storageDir, storageName)
        };

        await using var fs = new FileStream(excelModel.StoragePath, FileMode.Create, FileAccess.Write);

        //创建文件
        await _exportBuilder.Build(model, fs);

        return result.Success(excelModel);
    }

    public async Task<IResultModel<ExcelModel>> ExportByTemplate(string templatePath, IList<Dictionary<string, object>> properties)
    {
        if (templatePath.IsNull() || !File.Exists(templatePath))
            throw new NullReferenceException("导出模板不存在");

        if (properties == null || !properties.Any())
            throw new NullReferenceException("导出属性不存在");

        var result = new ResultModel<ExcelModel>();

        return result.Success();
    }

    /// <summary>
    /// 获取临时存储根路径
    /// </summary>
    /// <returns></returns>
    private string GetTempDirectory()
    {
        var tempPath = _options.CurrentValue.TempDir;

        if (tempPath.IsNull())
        {
            tempPath = _commonOptions.CurrentValue.TempDir;
        }

        if (tempPath.IsNull())
        {
            tempPath = Path.Combine(_commonOptions.CurrentValue.DefaultTempDir, "Excel");
        }
        else if (!Path.IsPathRooted(tempPath))
        {
            tempPath = Path.Combine(_commonOptions.CurrentValue.DefaultTempDir, tempPath);
        }

        return tempPath;
    }
}