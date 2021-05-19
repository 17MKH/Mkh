using System.Collections.Generic;
using Microsoft.Extensions.Hosting;

namespace Mkh.Module.Abstractions
{
    /// <summary>
    /// 模块集合
    /// </summary>
    public interface IModuleCollection : IList<ModuleDescriptor>
    {
        /// <summary>
        /// 环境
        /// </summary>
        public IHostEnvironment HostEnvironment { get; }

        /// <summary>
        /// 根据模块编号获取模块信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ModuleDescriptor Get(int id);

        /// <summary>
        /// 根据模块编码获取模块信息
        /// <para>不区分大小写</para>
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        ModuleDescriptor Get(string code);
    }
}
