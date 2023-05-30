using System.Threading.Tasks;
using Quartz;

namespace Mkh.TaskScheduler.Abstractions
{
    public interface ITask : IJob
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
       Task Execute(TaskExecutionContext context);
    }
}
