using System;

namespace Mkh.TaskScheduler.Abstractions
{
    /// <summary>
    /// 任务描述
    /// </summary>
    public interface ITaskDescriptor
    {
        /// <summary>
        /// 唯一编号，该编号动态生成
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 模块编码
        /// </summary>
        string ModuleCode { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        string DisplayName { get; set; }

        /// <summary>
        /// 类完整名称
        /// </summary>
        string ClassFullName { get; set; }
    }
}
