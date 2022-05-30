using System;

namespace Mkh.Data.Abstractions.Query
{
    /// <summary>
    /// 按月分表查询
    /// </summary>
    public interface IShardingByMonthQuery
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        [ShardingQueryValidation]
        DateTime StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        DateTime EndDate { get; set; }
    }
}
