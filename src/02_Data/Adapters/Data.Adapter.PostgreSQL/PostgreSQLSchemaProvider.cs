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
        var sql = @$"
select ordinal_position as Colorder
    ,column_name as Name
--     ,data_type as DataType
    ,c.typname as DataType
    ,case is_nullable when 'NO' then false else true end as IsNullable
    ,case when b.pk_name is null then false else true end as IsPrimaryKey
    ,column_default as DefaultValue
    ,c.DeText Description
    ,coalesce(character_maximum_length,numeric_precision,-1) as Length
    ,coalesce(character_maximum_length,numeric_precision,-1) as Precision
    ,numeric_scale as Precision
    ,case  when position('nextval' in column_default)>0 then true else false end as IsIdentity
from information_schema.columns 
left join (
    select pg_attr.attname as colname,pg_constraint.conname as pk_name from pg_constraint  
    inner join pg_class on pg_constraint.conrelid = pg_class.oid 
    inner join pg_attribute pg_attr on pg_attr.attrelid = pg_class.oid and  pg_attr.attnum = pg_constraint.conkey[1] 
    inner join pg_type on pg_type.oid = pg_attr.atttypid
    where pg_class.relname = '{table}' and pg_constraint.contype='p' 
) b on b.colname = information_schema.columns.column_name
left join (
    select attname,description as DeText, pg_type.typname from pg_class
    left join pg_attribute pg_attr on pg_attr.attrelid= pg_class.oid
    left join pg_description pg_desc on pg_desc.objoid = pg_attr.attrelid and pg_desc.objsubid=pg_attr.attnum
    left join pg_type on pg_type.oid = pg_attr.atttypid
    where pg_attr.attnum>0 and pg_attr.attrelid=pg_class.oid and pg_class.relname='{table}'
)c on c.attname = information_schema.columns.column_name
where table_schema='public' and table_name='{table}' order by ordinal_position asc";

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
                    case "text":
                    case "bpchar":
                    case "char":
                    case "varchar":
                        //schema.DefaultValue = schema.DefaultValue.Substring(3, schema.DefaultValue.Length - 5);
                        break;
                    case "date":
                    case "timestamp":
                    case "timestampz":
                        schema.DefaultValue = schema.DefaultValue.Replace("(", "").Replace(")", "");
                        if (schema.DefaultValue == "getdate")
                            schema.DefaultValue = "CURRENT_TIMESTAMP";
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
