using Mkh.Data.Abstractions.Descriptors;

namespace Mkh.Data.Abstractions.EntityChangeEvents
{
    /// <summary>
    /// 实体删除事件上下文
    /// </summary>
    public class EntityDeleteEventContext
    {
        /// <summary>
        /// 实体描述符
        /// </summary>
        public IEntityDescriptor EntityDescriptor { get; set; }

        /// <summary>
        /// 删除实体的编号
        /// </summary>
        public object Id { get; set; }
    }
}
