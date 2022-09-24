using System;
using Mkh.Data.Abstractions.Sharding;

namespace Mkh.Data.Abstractions.Annotations
{
    /// <summary>
    /// 启用分表
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ShardingAttribute : Attribute
    {
        /// <summary>
        /// 分表策略
        /// </summary>
        public ShardingPolicy Policy { get; set; }

        /// <summary>
        /// 自定义分表提供器类型
        /// </summary>
        public Type CustomProviderType { get; set; }

        /// <summary>
        /// 分表策略
        /// </summary>
        /// <param name="policy"></param>
        public ShardingAttribute(ShardingPolicy policy)
        {
            Policy = policy;
        }

        /// <summary>
        /// 自定义分表策略
        /// </summary>
        /// <param name="customProviderType"></param>
        public ShardingAttribute(Type customProviderType)
        {
            Policy = ShardingPolicy.Custom;
            CustomProviderType = customProviderType;
        }
    }
    
    /// <summary>
    /// 表示分表字段特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ShardingFieldAttribute : Attribute
    {
        protected bool _enable;

        /// <summary>
        ///  是否为分表字段
        /// </summary>
        public bool Enable
        {
            get
            {
                return this._enable;
            }
        }

        /// <summary>
        /// 分表字段
        /// </summary>
        /// <param name="enable"></param>
        public ShardingFieldAttribute(bool enable = true)
        {
            _enable = enable;
        }
    }
}
