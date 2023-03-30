using System;
using Microsoft.AspNetCore.Http;
using Mkh.Auth.Abstractions;
using Mkh.Data.Abstractions;

namespace Mkh.Auth.Core;

internal class OperatorResolver : IOperatorResolver
{
    private readonly IHttpContextAccessor _contextAccessor;

    public OperatorResolver(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public Guid? TenantId
    {
        get
        {
            var tenantId = _contextAccessor?.HttpContext?.User.FindFirst(MkhClaimTypes.TENANT_ID);

            if (tenantId != null && tenantId.Value.NotNull())
            {
                return new Guid(tenantId.Value);
            }

            return null;
        }
    }

    /// <summary>
    /// 账户编号
    /// </summary>
    public Guid? AccountId
    {
        get
        {
            var accountId = _contextAccessor?.HttpContext?.User?.FindFirst(MkhClaimTypes.ACCOUNT_ID);

            if (accountId != null && accountId.Value.NotNull())
            {
                return new Guid(accountId.Value);
            }

            return Guid.Empty;
        }
    }

    public string AccountName
    {
        get
        {
            var accountName = _contextAccessor?.HttpContext?.User?.FindFirst(MkhClaimTypes.ACCOUNT_NAME);

            if (accountName == null || accountName.Value.IsNull())
            {
                return "";
            }

            return accountName.Value;
        }
    }
}