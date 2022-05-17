﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mkh.Excel.Abstractions;

/// <summary>
/// Excel导出处理接口
/// </summary>
public interface IExcelExportHandler
{
    /// <summary>
    /// 创建导出的Excel文件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model"></param>
    /// <param name="entities"></param>
    /// <param name="stream">保存的流</param>
    /// <returns></returns>
    void CreateExcel<T>(ExportModel model, IList<T> entities, Stream stream);//where T : class, new();
}
