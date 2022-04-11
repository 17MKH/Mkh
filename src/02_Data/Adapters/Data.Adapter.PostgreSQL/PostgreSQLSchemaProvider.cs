using Dapper;
using Mkh.Data.Abstractions.Schema;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Mkh.Data.Adapter.PostgreSQL;

/// <summary>
/// PostgreSQL数据库架构提供器
/// </summary>
internal class PostgreSQLSchemaProvider : ISchemaProvider
{
    private readonly IDbConnection _con;

    public PostgreSQLSchemaProvider(IDbConnection con)
    {
        _con = con;
    }

    public PostgreSQLSchemaProvider(string connectionString)
    {
        _con = new NpgsqlConnection(connectionString);
    }

    public void CreateDatabase(string database)
    {
        var con = CreatePostgresConnection();
        con.ExecuteScalar($"CREATE DATABASE {database};");
        con.Close();
    }

    public List<ColumnSchema> GetColumns(string database, string table)
    {
        var sql = @$"select
ns.nspname,
c.relname,
a.attname as Name,
t.typname as DataType,
case when a.atttypmod > 0 and a.atttypmod < 32767 then a.atttypmod - 4 else a.attlen end Length,
case when t.typelem > 0 and t.typinput::varchar = 'array_in' then t2.typname else t.typname end,
case when a.attnotnull then false else true end as IsNullable,
--e.adsrc,
(select pg_get_expr(adbin, adrelid) from pg_attrdef where adrelid = e.adrelid limit 1) is_identity,
a.attndims,
coalesce(col_description(a.attrelid,a.attnum), '') as Description ,
case when pk.colName is not null then true else false end IsPrimaryKey,
pg_get_expr(d.adbin, d.adrelid) AS DefaultValue
from pg_class c
inner join pg_attribute a on a.attnum > 0 and a.attrelid = c.oid
inner join pg_type t on t.oid = a.atttypid
left join pg_type t2 on t2.oid = t.typelem
-- left join pg_description d on d.objoid = a.attrelid and d.objsubid = a.attnum
left join pg_attrdef e on e.adrelid = a.attrelid and e.adnum = a.attnum
inner join pg_namespace ns on ns.oid = c.relnamespace
inner join pg_namespace ns2 on ns2.oid = t.typnamespace
left join ( -- 取主键
	select pg_attribute.attname colName, * from pg_constraint
	inner join pg_attribute on pg_attribute.attrelid = pg_constraint.conrelid
	and  pg_attribute.attnum = pg_constraint.conkey[1]
	and pg_constraint.contype='p'
) pk
on pk.conrelid = c.oid and pk.colName = a.attname
-- 取默认值
LEFT JOIN pg_catalog.pg_attrdef d ON (a.attrelid, a.attnum) = (d.adrelid,  d.adnum)
-- where ns.nspname = '{database}' and c.relname = '{table}'
where c.relname = '{table}'";

        var list = _con.Query<ColumnSchema>(sql).ToList();

        foreach (var schema in list)
        {
            if (schema.DefaultValue.NotNull())
            {
                switch (schema.DataType)
                {
                    case "int":
                    case "decimal":
                    case "float4":
                    case "float8":
                    case "numeric":
                    case "smallint":
                        //schema.DefaultValue = schema.DefaultValue.Substring(2, schema.DefaultValue.Length - 4);
                        break;
                    case "bool":
                        if (bool.TryParse(schema.DefaultValue, out var val))
                        {
                            schema.DefaultValue = val.ToString();
                        }
                        break;
                    case "varchar":
                        schema.DefaultValue = schema.DefaultValue.Substring(3, schema.DefaultValue.Length - 5);
                        break;
                    case "date":
                    case "timestamp":
                    case "timestampz":
                        schema.DefaultValue = schema.DefaultValue.Replace("(", "").Replace(")", "");
                        if (schema.DefaultValue == "getdate")
                            schema.DefaultValue = "now()";
                        break;
                }
            }
        }

        return list;
    }

    public DatabaseSchema GetDatabase(string database)
    {
        //var sql = $"SELECT datname as Name FROM pg_catalog.pg_database WHERE datname = '{database}';";
        var sql = $"select * from pg_namespace where nspname = '{database}'";

        var db = _con.QueryFirstOrDefault<DatabaseSchema>(sql);
        if (db != null)
        {
            db.Tables = GetTables(database);
        }

        return db;
    }

    public List<DatabaseSchema> GetDatabases()
    {
        //var sql = "SELECT datname as Name FROM pg_catalog.pg_database WHERE datname NOT IN ('postgres');";
        var sql = "select * from pg_namespace where nspname not in ('information_schema', 'pg_catalog', 'pg_toast', 'pg_temp_1', 'pg_toast_temp_1')";
        var databases = _con.Query<DatabaseSchema>(sql).ToList();
        foreach (var database in databases)
        {
            database.Tables = GetTables(database.Name);
        }
        return databases;
    }

    public List<TableSchema> GetTables(string database)
    {
        //var sql = $"select tablename as Name from pg_tables where schemaname = '{database}'";
        var sql = $"select tablename as Name from pg_tables";
        var tables = _con.Query<TableSchema>(sql).ToList();
        foreach (var table in tables)
        {
            table.Columns = GetColumns(database, table.Name);
        }

        return tables;
    }

    public bool IsExistsDatabase(string database)
    {
        var con = CreatePostgresConnection();
        var exists = con.ExecuteScalar($"SELECT 1::int FROM pg_catalog.pg_database u where u.datname='{database}';").ToInt() > 0;
        con.Close();
        return exists;
    }

    public bool IsExistsTable(string database, string table)
    {
        return _con.ExecuteScalar($"select 1 from pg_tables a inner join pg_namespace b on b.nspname = a.schemaname where a.tablename = '{table}';").ToInt() > 0;
    }

    private NpgsqlConnection CreatePostgresConnection()
    {
        var builder = new NpgsqlConnectionStringBuilder(_con.ConnectionString)
        {
            Database = "postgres"
        };

        var con = new NpgsqlConnection(builder.ToString());
        con.Open();

        return con;
    }
}
