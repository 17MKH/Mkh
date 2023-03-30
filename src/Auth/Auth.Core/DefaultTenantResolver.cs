using System;
using System.Threading.Tasks;
using Mkh.Auth.Abstractions;

namespace Mkh.Auth.Core
{
    public class DefaultTenantResolver : ITenantResolver
    {
        public Task<Guid?> ResolveId()
        {
            return Task.FromResult<Guid?>(null);
        }

        public Task<string> GetTenantName(Guid? id)
        {
            return Task.FromResult(string.Empty);
        }
    }
}
