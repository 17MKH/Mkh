using System.Collections.Generic;

namespace Mkh.Excel.Abstractions.Export
{
    /// <summary>
    /// Excel导出模板模型
    /// </summary>
    public class ExcelExportTemplateModel
    {
        /// <summary>
        /// 模板属性列表
        /// </summary>
        public IList<Dictionary<string, object>> Properties { get; set; }

        /// <summary>
        /// 模板路径
        /// </summary>
        public string TemplatePath { get; set; }
    }
}
