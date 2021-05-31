namespace Mkh.Module.Abstractions.Options
{
    /// <summary>
    /// 模块配置项
    /// </summary>
    public class ModuleOptions
    {
        /// <summary>
        /// 模块编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 数据库配置项
        /// </summary>
        public ModuleDbOptions Db { get; set; }
    }
}
