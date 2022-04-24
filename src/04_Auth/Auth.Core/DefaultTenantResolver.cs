using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mkh.Auth.Abstractions;

namespace Mkh.Auth.Core
{
    public class DefaultTenantResolver : ITenantResolver
    {
        public Task<Guid?> Resolve()
        {
            throw new NotImplementedException();
        }
    }
}
