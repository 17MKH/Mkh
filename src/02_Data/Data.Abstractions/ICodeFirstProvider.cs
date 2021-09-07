namespace Mkh.Data.Abstractions
{
    /// <summary>
    /// 代码优先提供器
    /// </summary>
    public interface ICodeFirstProvider
    {
        /// <summary>
        /// 创建库
        /// </summary>
        bool CreateDatabase();

        /// <summary>
        /// 创建表
        /// </summary>
        void CreateTable();

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="repositoryManager">仓储管理器</param>
        void InitData(IRepositoryManager repositoryManager);
    }
}
