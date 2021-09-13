using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mkh.Mod.Admin.Core.Infrastructure.Defaults
{
    internal class DefaultCredentialClaimExtender : ICredentialClaimExtender
    {
        public Task Extend(List<Claim> claims, Guid accountId)
        {
            return Task.CompletedTask;
        }
    }
}
