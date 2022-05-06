using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mkh.Data.Abstractions.Annotations
{
    /// <summary>
    /// 分表
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ShardingAttribute : Attribute
    {

    }
}
