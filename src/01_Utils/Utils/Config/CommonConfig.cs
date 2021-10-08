namespace Mkh.Utils.Config
{
    /// <summary>
    /// 通用配置
    /// </summary>
    public class CommonConfig : IConfig
    {
        /// <summary>
        /// 临时目录，默认是应用程序根目录下的Temp目录
        /// </summary>
        public string TempDir { get; set; }
    }
}
