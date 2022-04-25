namespace Mkh.Auth.Abstractions
{
    /// <summary>
    /// 平台类型集合
    /// </summary>
    public class PlatformCollection
    {
        /// <summary>
        /// Web平台
        /// </summary>
        public PlatformDescriptor Web { get; }

        /// <summary>
        /// Android平台
        /// </summary>
        public PlatformDescriptor Android { get; }

        public PlatformCollection()
        {
            Web = new() { Name = "Web", Value = 1 };
            Android = new() { Name = "安卓", Value = 2 };
        }
    }

    /// <summary>
    /// 平台描述
    /// </summary>
    public class PlatformDescriptor
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public int Value { get; set; }
    }
}
