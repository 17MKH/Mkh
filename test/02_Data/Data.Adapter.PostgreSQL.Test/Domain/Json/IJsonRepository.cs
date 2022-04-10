using Mkh.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Adapter.PostgreSQL.Test.Domain.Json;

public interface IJsonRepository : IRepository<JsonEntity>
{
}
