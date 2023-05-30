using System;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Sharding;

namespace Mkh.Data.Core.Sharding.Providers
{
    /// <summary>
    /// 季度分表策略提供器
    /// </summary>
    internal class QuarterShardingPolicyProvider : IShardingPolicyProvider
    {
        public string ResolveTableName(IEntityDescriptor descriptor, DateTime dateTime, bool next = false)
        {
            var year = dateTime.Year;
            var month = dateTime.Month;
            var quarter = 1;

            if (month >= 1 && month < 4)
                quarter = 1;
            else if (month >= 4 && month < 7)
                quarter = 2;
            else if (month >= 7 && month < 10)
                quarter = 3;
            else if (month >= 10)
                quarter = 4;

            if (next)
            {
                if (quarter == 4)
                {
                    year++;
                    quarter = 1;
                }
                else
                {
                    quarter++;
                }
            }

            return $"{descriptor.TableName}_{year}{quarter}";
        }
    }
}
