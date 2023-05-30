using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;
using Mkh.Data.Abstractions.Schema;

namespace Mkh.Data.Adapter.Sqlite;

/// <summary>
/// MySql数据库架构提供器
/// </summary>
public class SqliteSchemaProvider : ISchemaProvider
{
    private readonly IDbConnection _con;
    public SqliteSchemaProvider(IDbConnection con)
    {
        _con = con;
    }

    public SqliteSchemaProvider(string connectionString)
    {
        _con = new SqliteConnection(connectionString);
    }

    public bool IsExistsDatabase(string database)
    {
        throw new Exception("Sqlite不支持该方法~");
    }

    public bool IsExistsTable(string database, string table)
    {
        var sql = $"SELECT 1 FROM sqlite_master WHERE tbl_name='{table}' AND type='table' LIMIT 1;";
        return _con.ExecuteScalar<int>(sql) > 0;
    }

    public List<DatabaseSchema> GetDatabases()
    {
        throw new Exception("Sqlite不支持该方法~");
    }

    public DatabaseSchema GetDatabase(string database)
    {
        throw new Exception("Sqlite不支持该方法~");
    }

    public List<TableSchema> GetTables(string database)
    {
        var sql = "SELECT tbl_name AS [Name] FROM sqlite_master WHERE type='table';";
        var tables = _con.Query<TableSchema>(sql).ToList();
        foreach (var table in tables)
        {
            table.Columns = GetColumns(database, table.Name);
        }

        return tables;
    }

    public List<ColumnSchema> GetColumns(string database, string table)
    {
        var sql = $"PRAGMA table_info([{table}])";
        var result = _con.Query(sql);
        var columns = new List<ColumnSchema>();
        foreach (var r in result)
        {
            columns.Add(new ColumnSchema
            {
                Name = r.name,
                DataType = r.type,
                IsNullable = r.notnull,
                DefaultValue = r.dflt_value,
                IsPrimaryKey = r.pk
            });
        }

        return columns;
    }

    public void CreateDatabase(string database)
    {
        throw new Exception("Sqlite不支持该方法~");
    }
}