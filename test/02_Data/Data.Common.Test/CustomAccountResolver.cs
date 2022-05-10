using System;
using Mkh.Data.Abstractions;

namespace Data.Common.Test
{
    /// <summary>
    /// 自定义账户信息解析器
    /// </summary>
    public class CustomAccountResolver : IOperatorResolver
    {
        public Guid? TenantId => Guid.Parse("39f08cfd-8e0d-771b-a2f3-2639a62ca2fa");
        public Guid? AccountId => Guid.Parse("49f08cfd-8e0d-771b-a2f3-2639a62ca2fa");
        public string AccountName => "OLDLI";
    }
}
