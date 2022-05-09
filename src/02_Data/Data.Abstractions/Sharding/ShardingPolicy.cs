namespace Mkh.Data.Abstractions.Sharding
{
    /// <summary>
    /// 分表策略
    /// </summary>
    public enum ShardingPolicy
    {
        /// <summary>
        /// 自定义
        /// </summary>
        Custom,
        /// <summary>
        /// 按年度分表
        /// </summary>
        Year,
        /// <summary>
        /// 按季度分表
        /// </summary>
        Quarter,
        /// <summary>
        /// 按月份分表
        /// </summary>
        Month,
        /// <summary>
        /// 按天分表
        /// </summary>
        Day,
    }
}
