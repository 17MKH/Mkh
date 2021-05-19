using System;

namespace Mkh.Data.Abstractions.Entities
{
    /// <summary>
    /// 实体软删除扩展
    /// </summary>
    public interface ISoftDelete<TDeletedByKey>
    {
        /// <summary>
        /// 已删除的
        /// </summary>
        bool Deleted { get; set; }

        /// <summary>
        /// 删除人编号
        /// </summary>
        TDeletedByKey DeletedBy { get; set; }

        /// <summary>
        /// 删除人名称
        /// </summary>
        string Deleter { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        DateTime? DeletedTime { get; set; }
    }

    /// <summary>
    /// 实体软删除扩展，删除人主键类型为Int
    /// </summary>
    public interface ISoftDelete : ISoftDelete<Guid?>
    {

    }
}
