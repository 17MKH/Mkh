using System;
using Quartz;

namespace Mkh.TaskScheduler.Abstractions
{
    /// <summary>
    /// 任务执行上下文
    /// </summary>
    public class TaskExecutionContext
    {
        /// <summary>
        /// 任务编号
        /// </summary>
        public Guid TaskId { get; set; }

        public IJobExecutionContext JobExecutionContext { get; set; }
    }
}
