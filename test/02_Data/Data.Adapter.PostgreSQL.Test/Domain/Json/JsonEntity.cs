using Mkh.Data.Abstractions.Annotations;
using Mkh.Data.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Adapter.PostgreSQL.Test.Domain.Json;

[Table("json_test")]
public class JsonEntity : EntityBaseSoftDelete<Guid>
{
    [Column("body", Type = "jsonb")]
    public string Body { get; set; }
}
