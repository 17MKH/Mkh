using Data.Adapter.PostgreSQL.Test.Domain.Json;
using Mkh.Data.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Adapter.PostgreSQL.Test.Infrastructure.Repositories;

internal class JsonRepository : RepositoryAbstract<JsonEntity>, IJsonRepository
{
}
