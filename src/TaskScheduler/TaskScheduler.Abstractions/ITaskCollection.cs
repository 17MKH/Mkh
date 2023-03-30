using System.Collections.Generic;

namespace Mkh.TaskScheduler.Abstractions
{
    /// <summary>
    /// 任务
    /// </summary>
    public interface ITaskCollection : IList<ITaskDescriptor>
    {
    }
}
