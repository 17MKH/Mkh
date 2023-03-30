using System;
using Mkh.TaskScheduler.Abstractions;

namespace Mkh.TaskScheduler.Core
{
    internal class TaskDescriptor : ITaskDescriptor
    {
        /// <summary>
        /// 唯一编号，该编号动态生成
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// 模块编码
        /// </summary>
        public string ModuleCode { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 类完整名称
        /// </summary>
        public string ClassFullName { get; set; }

        public TaskDescriptor()
        {
            Id = Guid.NewGuid();
        }
    }
}
