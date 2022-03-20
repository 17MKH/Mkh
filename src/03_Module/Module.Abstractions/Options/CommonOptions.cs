namespace Mkh.Module.Abstractions.Options
{
    /// <summary>
    /// 通用配置
    /// </summary>
    public class CommonOptions
    {
        /// <summary>
        /// 临时目录，默认是应用程序根目录下的Temp目录
        /// </summary>
        public string TempDir { get; set; }

        /// <summary>
        /// 数据库配置
        /// </summary>
        public ModuleDbOptions Db { get; set; }
    }
}
