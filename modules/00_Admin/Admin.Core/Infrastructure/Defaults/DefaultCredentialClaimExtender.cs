using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Mkh.Utils.Annotations;

namespace Mkh.Mod.Admin.Core.Infrastructure.Defaults;

[ScopedInject]
internal class DefaultCredentialClaimExtender : ICredentialClaimExtender
{
    public Task Extend(List<Claim> claims, Guid accountId)
    {
        return Task.CompletedTask;
    }
}