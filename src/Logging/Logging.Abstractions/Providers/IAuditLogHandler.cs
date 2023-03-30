using System;
using System.Threading.Tasks;

namespace Mkh.Logging.Abstractions.Providers
{
    /// <summary>
    /// 审计日志处理器
    /// </summary>
    public interface IAuditLogHandler
    {
        /// <summary>
        /// 写入审计日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> Write(AuditLogModel model);
    }

    /// <summary>
    /// 审计日志模型
    /// </summary>
    public class AuditLogModel : BaseLogModel
    {
        /// <summary>
        /// 模块编码
        /// </summary>
        public string ModuleCode { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 控制器
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// 操作
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 参数(Json序列化)
        /// </summary>
        public string Parameters { get; set; }

        /// <summary>
        /// 返回值(Json序列化)
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 方法开始执行时间
        /// </summary>
        public DateTime ExecutionTime { get; set; }

        /// <summary>
        /// 方法执行总用时(ms)
        /// </summary>
        public long ExecutionDuration { get; set; }

        /// <summary>
        /// 浏览器UI
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        public string IP { get; set; }
    }
}
