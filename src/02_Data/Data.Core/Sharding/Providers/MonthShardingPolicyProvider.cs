using System;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Sharding;

namespace Mkh.Data.Core.Sharding.Providers
{
    /// <summary>
    /// 月份分表提供器
    /// </summary>
    internal class MonthShardingPolicyProvider : IShardingPolicyProvider
    {
        public string ResolveTableName(IEntityDescriptor descriptor, DateTime dateTime, bool next = false)
        {
            if (next)
            {
                dateTime = dateTime.AddMonths(1);
            }

            return $"{descriptor.TableName}_{dateTime:yyyyMM}";
        }
    }
}
