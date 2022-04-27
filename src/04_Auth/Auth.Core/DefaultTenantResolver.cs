using System;
using System.Threading.Tasks;
using Mkh.Auth.Abstractions;

namespace Mkh.Auth.Core
{
    public class DefaultTenantResolver : ITenantResolver
    {
        public Task<Guid?> Resolve()
        {
            return Task.FromResult<Guid?>(null);
        }
    }
}
