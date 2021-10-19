using System;
using Dapper;

namespace Mkh.Data.Adapter.Sqlite;

public class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
{
    public override Guid Parse(object value)
    {
        return Guid.Parse(value.ToString());
    }

    public override void SetValue(System.Data.IDbDataParameter parameter, Guid value)
    {
        parameter.Value = value.ToString();
    }
}