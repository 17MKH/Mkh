using System;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Sharding;

namespace Mkh.Data.Core.Sharding.Providers
{
    /// <summary>
    /// 按年度分表提供器
    /// </summary>
    internal class YearShardingPolicyProvider : IShardingPolicyProvider
    {
        public string ResolveTableName(IEntityDescriptor descriptor, DateTime dateTime, bool next = false)
        {
            var year = dateTime.Year;

            if (next)
            {
                year++;
            }

            return $"{descriptor.TableName}_{year}";
        }
    }
}
