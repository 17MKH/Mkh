using Mkh.Auth.Abstractions;
using Mkh.Module.Abstractions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mkh.Excel.Abstractions;

public abstract class ExcelHandlerAbstract : IExcelHandler
{
    protected readonly IAccount LoginInfo;
    private readonly IExcelExportHandler _exportHandler;
    private readonly ExcelConfig _config;
    private readonly CommonOptions _commonOptions;

    //导出Excel的对象的属性类型列表
    private readonly ConcurrentDictionary<RuntimeTypeHandle, Dictionary<string, PropertyInfo>> _exportObjectProperties = new ConcurrentDictionary<RuntimeTypeHandle, Dictionary<string, PropertyInfo>>();

    protected ExcelHandlerAbstract(IAccount loginInfo, IExcelExportHandler exportHandler, ExcelConfig config, CommonOptions commonOptions)
    {
        LoginInfo = loginInfo;
        _exportHandler = exportHandler;
        _config = config;
        _commonOptions = commonOptions;
    }

    public ExcelExportResultModel Export<T>(ExportModel model, IList<T> entities) //where T : class, new()
    {
        if (model == null)
            throw new NullReferenceException("Excel导出信息不存在");

        //设置列对应的属性类型
        // DynamicObject和JObject、JsonObject不使用反射处理
        if (typeof(T).GetInterfaces()?.Contains(typeof(IDictionary<,>)) == true)
        {
            SetColumnPropertyType<T>(model);
        }

        if (_config.TempPath.IsNull())
        {
            _config.TempPath = Path.Combine(_commonOptions.TempDir, "Excel");
        }
        if (!System.IO.Path.IsPathRooted(_config.TempPath))
        {
            _config.TempPath = Path.Combine(AppContext.BaseDirectory, _config.TempPath);
        }

        var saveName = Guid.NewGuid() + model.Format.ToDescription();

        var saveDir = Path.Combine(_config.TempPath, "Export", DateTime.Now.Format("yyyyMMdd"));

        if (!Directory.Exists(saveDir))
        {
            Directory.CreateDirectory(saveDir);
        }

        var result = new ExcelExportResultModel
        {
            SaveName = saveName,
            FileName = model.FileName,
            Path = Path.Combine(saveDir, saveName)
        };

        using var fs = new FileStream(result.Path, FileMode.Create, FileAccess.Write);

        //创建文件
        _exportHandler.CreateExcel(model, entities, fs);

        return result;
    }

    /// <summary>
    /// 设置列属性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model"></param>
    private void SetColumnPropertyType<T>(ExportModel model) //where T : class, new()
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
                types.Add(property.Name.ToLower(), property);
            }

            _exportObjectProperties.TryAdd(objectType.TypeHandle, types);
        }

        foreach (var column in model.Columns)
        {
            column.PropertyInfo = types.FirstOrDefault(m => m.Key.EqualsIgnoreCase(column.Name)).Value;
        }
    }
}