using System;
using Mkh.Data.Abstractions.Descriptors;

namespace Mkh.Data.Abstractions.Sharding
{
    /// <summary>
    /// 分表策略提供器
    /// </summary>
    public interface IShardingPolicyProvider
    {
        /// <summary>
        /// 根据指定日期解析表名称
        /// </summary>
        /// <param name="descriptor">实体描述符</param>
        /// <param name="dateTime">时间</param>
        /// <param name="next">解析下一张表</param>
        /// <returns></returns>
        string ResolveTableName(IEntityDescriptor descriptor, DateTime dateTime, bool next = false);
    }
}
