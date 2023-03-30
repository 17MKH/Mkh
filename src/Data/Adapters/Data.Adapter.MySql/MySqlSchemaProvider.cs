using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Mkh.Data.Abstractions.Schema;
using MySqlConnector;

namespace Mkh.Data.Adapter.MySql;

/// <summary>
/// MySql数据库架构提供器
/// </summary>
public class MySqlSchemaProvider : ISchemaProvider
{
    private readonly IDbConnection _con;
    public MySqlSchemaProvider(IDbConnection con)
    {
        _con = con;
    }

    public MySqlSchemaProvider(string connectionString)
    {
        _con = new MySqlConnection(connectionString);
    }

    public bool IsExistsDatabase(string database)
    {
        var sql = $"SELECT 1 FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = '{database}' LIMIT 1;";
        return _con.ExecuteScalar<int>(sql) > 0;
    }

    public bool IsExistsTable(string database, string table)
    {
        var sql = $"SELECT 1 FROM information_schema.TABLES WHERE TABLE_NAME ='{table}' and TABLE_SCHEMA='{database}';";
        return _con.ExecuteScalar<int>(sql) > 0;
    }

    public List<DatabaseSchema> GetDatabases()
    {
        var sql = "SELECT SCHEMA_NAME AS `Name` FROM INFORMATION_SCHEMA.SCHEMATA;";
        var databases = _con.Query<DatabaseSchema>(sql).ToList();
        foreach (var database in databases)
        {
            database.Tables = GetTables(database.Name);
        }
        return databases;
    }

    public DatabaseSchema GetDatabase(string database)
    {
        var sql = $"SELECT SCHEMA_NAME AS `Name` FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = '{database}';";

        var db = _con.QueryFirstOrDefault<DatabaseSchema>(sql);
        if (db != null)
        {
            db.Tables = GetTables(database);
        }

        return db;
    }

    public List<TableSchema> GetTables(string database)
    {
        var sql = $"SELECT TABLE_NAME AS `Name` FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='{database}'";
        var tables = _con.Query<TableSchema>(sql).ToList();
        foreach (var table in tables)
        {
            table.Columns = GetColumns(database, table.Name);
        }

        return tables;
    }

    public List<ColumnSchema> GetColumns(string database, string table)
    {
        var sql = @$"SELECT COLUMN_NAME AS `Name`,DATA_TYPE AS `DataType`,CASE IS_NULLABLE WHEN 'YES' THEN 1 ELSE 0 END AS `IsNullable`,CASE COLUMN_KEY WHEN 'PRI' THEN 1 ELSE 0 END AS `IsPrimaryKey`,
            COLUMN_DEFAULT AS `DefaultValue`,COLUMN_COMMENT AS `Description`,CHARACTER_MAXIMUM_LENGTH AS `Length`,NUMERIC_PRECISION AS `Precision`,NUMERIC_SCALE AS `Scale`
            FROM `INFORMATION_SCHEMA`.`COLUMNS` WHERE TABLE_SCHEMA = '{database}' AND TABLE_NAME = '{table}'; ";

        return _con.Query<ColumnSchema>(sql).ToList();
    }

    public void CreateDatabase(string database)
    {
        var sql = $"CREATE DATABASE {database} CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;";
        _con.Execute(sql);
    }
}