using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Mkh.Module.Abstractions;
using Mkh.TaskScheduler.Abstractions;

namespace Mkh.TaskScheduler.Core
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加任务调度功能
        /// </summary>
        /// <param name="services"></param>
        /// <param name="modules"></param>
        /// <returns></returns>
        public static IServiceCollection AddTaskScheduler(this IServiceCollection services, IModuleCollection modules)
        {
            var taskCollection = new TaskCollection();

            foreach (var module in modules)
            {
                var jobTypes = module.LayerAssemblies.Core.GetTypes().Where(m => typeof(ITask).IsImplementType(m)).ToList();
                if (jobTypes.Any())
                {
                    foreach (var jobType in jobTypes)
                    {
                        var taskDescriptor = new TaskDescriptor
                        {
                            ModuleCode = module.Code,
                            DisplayName = jobType.Name,
                            ClassFullName = $"{jobType.FullName}, {jobType.Assembly.GetName().Name}"
                        };

                        var displayNameAttribute = jobType.GetCustomAttribute(typeof(DisplayNameAttribute));
                        if (displayNameAttribute != null)
                        {
                            taskDescriptor.DisplayName = ((DisplayNameAttribute)displayNameAttribute).DisplayName;
                        }

                        taskCollection.Add(taskDescriptor);

                        services.AddTransient(jobType);
                    }
                }
            }

            services.AddSingleton<ITaskCollection>(taskCollection);

            return services;
        }
    }
}
