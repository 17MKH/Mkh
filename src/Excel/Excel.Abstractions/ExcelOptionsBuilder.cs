using Microsoft.Extensions.DependencyInjection;

namespace Mkh.Excel.Abstractions
{
    /// <summary>
    /// 用于给Provider提供配置
    /// </summary>
    public class ExcelOptionsBuilder
    {
        /// <summary>
        /// 配置项
        /// </summary>
        public ExcelOptions Options { get; set; }

        /// <summary>
        /// 服务集合
        /// </summary>
        public IServiceCollection Services { get; set; }
    }
}
