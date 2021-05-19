using System;

namespace Mkh.Data.Abstractions.Annotations
{
    /// <summary>
    /// 小数精度
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PrecisionAttribute : Attribute
    {
        /// <summary>
        /// 长度
        /// </summary>
        public int M { get; set; }

        /// <summary>
        /// 小数位长度
        /// </summary>
        public int D { get; set; }

        public PrecisionAttribute(int m, int d)
        {
            M = m;
            D = d;
        }

        public PrecisionAttribute()
        {
            M = 18;
            D = 4;
        }
    }
}
