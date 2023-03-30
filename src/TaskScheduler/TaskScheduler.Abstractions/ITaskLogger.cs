using System;
using System.Threading.Tasks;

namespace Mkh.TaskScheduler.Abstractions
{
    /// <summary>
    /// 任务日志记录器
    /// </summary>
    public interface ITaskLogger
    {
        /// <summary>
        /// 任务编号
        /// </summary>
        Guid JobId { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="msg">消息</param>
        Task Info(string msg);

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        Task Debug(string msg);

        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        Task Error(string msg);
    }
}
