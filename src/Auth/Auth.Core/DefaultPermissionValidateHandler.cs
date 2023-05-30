using System.Collections.Generic;
using System.Threading.Tasks;
using Mkh.Auth.Abstractions;

namespace Mkh.Auth.Core
{
    internal class DefaultPermissionValidateHandler : IPermissionValidateHandler
    {
        public Task<bool> Validate(IDictionary<string, object> routeValues, HttpMethod httpMethod)
        {
            return Task.FromResult(true);
        }
    }
}
