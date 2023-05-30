using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Mkh.Data.Abstractions.Schema;

namespace Mkh.Data.Adapter.SqlServer;

/// <summary>
/// SqlServer数据库架构提供器
/// </summary>
public class SqlServerSchemaProvider : ISchemaProvider
{
    private readonly IDbConnection _con;

    public SqlServerSchemaProvider(IDbConnection con)
    {
        _con = con;
    }

    public SqlServerSchemaProvider(string connectionString)
    {
        _con = new SqlConnection(connectionString);
    }

    public bool IsExistsDatabase(string database)
    {
        var sql = $"SELECT TOP 1 1 FROM master.dbo.SysDatabases WHERE NAME = '{database}'";
        return _con.ExecuteScalar<int>(sql) > 0;
    }

    public bool IsExistsTable(string database, string table)
    {
        _con.Open();
        _con.Execute($"USE {database};");
        var sql = $"SELECT 1 FROM dbo.SysObjects WHERE xtype='U' AND Name='{table}';";
        var exists = _con.ExecuteScalar<int>(sql) > 0;
        _con.Close();
        return exists;
    }

    public List<DatabaseSchema> GetDatabases()
    {
        var sql = "SELECT Name FROM master.dbo.SysDatabases WHERE Name NOT IN ('master','model','msdb','tempdb');";
        var databases = _con.Query<DatabaseSchema>(sql).ToList();
        foreach (var database in databases)
        {
            database.Tables = GetTables(database.Name);
        }
        return databases;
    }

    public DatabaseSchema GetDatabase(string database)
    {
        var sql = $"SELECT Name FROM master.dbo.SysDatabases WHERE Name = '{database}';";

        var db = _con.QueryFirstOrDefault<DatabaseSchema>(sql);
        if (db != null)
        {
            db.Tables = GetTables(database);
        }

        return db;
    }

    public List<TableSchema> GetTables(string database)
    {
        _con.Open();
        _con.Execute($"USE {database};");

        var sql = "SELECT Name FROM dbo.SysObjects WHERE xtype='U';";
        var tables = _con.Query<TableSchema>(sql).ToList();
        foreach (var table in tables)
        {
            table.Columns = GetColumns(database, table.Name);
        }
        _con.Close();
        return tables;
    }

    public List<ColumnSchema> GetColumns(string database, string table)
    {
        _con.Open();
        _con.Execute($"USE {database};");
        var sql = @$"SELECT T1.name AS [Name],T5.name AS [DataType],T1.isnullable AS [IsNullable],CASE WHEN T4.id IS NULL THEN 0 ELSE 1 END AS [IsPrimaryKey],
                        T7.text AS [DefaultValue],CAST(ISNULL(T6.value,'') AS NVARCHAR(2000)) [Description],T1.prec AS [Length],T1.xprec AS [Precision],T1.xscale AS [Scale]
                        FROM SYSCOLUMNS T1
                        LEFT JOIN SYSOBJECTS T2 on  T2.parent_obj = T1.id  AND T2.xtype = 'PK' 
                        LEFT JOIN SYSINDEXES T3 on  T3.id = T1.id  AND T2.name = T3.name  
                        LEFT JOIN SYSINDEXKEYS T4 on T1.colid = T4.colid AND T4.id = T1.id AND T4.indid = T3.indid
                        LEFT JOIN systypes  T5 on  T1.xtype = T5.xtype
                        LEFT JOIN sys.extended_properties T6 ON T1.id = T6.major_id AND T1.colid = T6.minor_id
                        LEFT JOIN syscomments AS T7 ON T1.cdefault = T7.id
                        WHERE T1.id = object_id('{table}') AND T5.name <> 'sysname';";

        var list = _con.Query<ColumnSchema>(sql).ToList();
        _con.Close();

        foreach (var schema in list)
        {
            if (schema.DefaultValue.NotNull())
            {
                switch (schema.DataType)
                {
                    case "int":
                    case "bit":
                    case "decimal":
                    case "float":
                    case "smallint":
                        schema.DefaultValue = schema.DefaultValue.Substring(2, schema.DefaultValue.Length - 4);
                        break;
                    case "nvarchar":
                        schema.DefaultValue = schema.DefaultValue.Substring(3, schema.DefaultValue.Length - 5);
                        break;
                    case "datetime":
                        schema.DefaultValue = schema.DefaultValue.Replace("(", "").Replace(")", "");
                        if (schema.DefaultValue == "getdate")
                            schema.DefaultValue = "getdate()";
                        break;
                }
            }
        }

        return list;
    }

    public void CreateDatabase(string database)
    {
        var sql = $"CREATE DATABASE [{database}]";
        _con.Execute(sql);
    }
}