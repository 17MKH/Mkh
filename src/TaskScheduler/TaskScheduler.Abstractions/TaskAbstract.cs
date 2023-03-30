using System;
using System.Threading.Tasks;
using Quartz;

namespace Mkh.TaskScheduler.Abstractions
{
    /// <summary>
    /// 任务抽象类
    /// </summary>
    public abstract class TaskAbstract : ITask
    {
        protected readonly ITaskLogger Logger;

        protected TaskAbstract(ITaskLogger logger)
        {
            Logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var jobId = context.JobDetail.JobDataMap["id"];
            Logger.JobId = jobId == null ? Guid.Empty : Guid.Parse(jobId.ToString());

            await Logger.Info("Task begin execute");

            try
            {
                await Execute(new TaskExecutionContext
                {
                    TaskId = Logger.JobId,
                    JobExecutionContext = context
                });
            }
            catch (Exception ex)

            {
                await Logger.Error("Task error：" + ex);
            }

            await Logger.Info("Task end");
        }

        public abstract Task Execute(TaskExecutionContext context);
    }
}
