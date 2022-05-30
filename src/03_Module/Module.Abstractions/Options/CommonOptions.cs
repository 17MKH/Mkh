using System;
using System.IO;

namespace Mkh.Module.Abstractions.Options
{
    /// <summary>
    /// 通用配置
    /// </summary>
    public class CommonOptions
    {
        /// <summary>
        /// 默认临时文件
        /// </summary>
        public string DefaultTempDir => Path.Combine(AppContext.BaseDirectory, "Temp");

        /// <summary>
        /// 临时目录，默认是应用程序根目录下的Temp目录
        /// </summary>
        public string TempDir { get; set; }

        /// <summary>
        /// 默认语言
        /// </summary>
        public string Lang { get; set; }

        /// <summary>
        /// 数据库配置
        /// </summary>
        public ModuleDbOptions Db { get; set; }
    }
}
