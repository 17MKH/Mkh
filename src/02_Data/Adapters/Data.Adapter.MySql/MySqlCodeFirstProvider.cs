using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Options;
using Mkh.Data.Abstractions.Schema;
using Mkh.Data.Core;

namespace Mkh.Data.Adapter.MySql;

public class MySqlCodeFirstProvider : CodeFirstProviderAbstract
{
    public MySqlCodeFirstProvider(CodeFirstOptions options, IDbContext context, IServiceCollection service) : base(options, context, service)
    {
    }

    #region ==创建库==

    public override void CreateDatabase()
    {
        var con = Context.NewConnection();

        //数据库名称
        var databaseName = con.Database;
        /*
        * 创建数据库需要当前连接字符串使用的账户拥有对应的权限，并且需要使用mysql数据库
        */
        var connString = con.ConnectionString.Replace(databaseName, "mysql");
        using var con1 = Context.NewConnection(connString);
        var schemaProvider = new MySqlSchemaProvider(con1);
        var isExistsDatabase = schemaProvider.IsExistsDatabase(databaseName);
        if (isExistsDatabase)
        {
            con.Close();
            return;
        }

        //创建前事件
        Options.BeforeCreateDatabase?.Invoke(Context);

        //创建数据库
        schemaProvider.CreateDatabase(databaseName);

        con1.Close();

        //创建后事件
        Options.AfterCreateDatabase?.Invoke(Context);
    }

    #endregion

    #region ==创建表==

    public override void CreateTable()
    {
        foreach (var descriptor in Context.EntityDescriptors.Where(m => m.AutoCreate))
        {
            CreateTable(descriptor);
        }
    }

    public override void CreateNextTable()
    {
        var shardingEntities = Context.EntityDescriptors.Where(m => m.AutoCreate && m.IsSharding).ToList();
        if (shardingEntities.Any())
        {
            foreach (var entity in shardingEntities)
            {
                CreateTable(entity, true);
            }
        }
    }

    /// <summary>
    /// 创建表
    /// </summary>
    /// <param name="descriptor"></param>
    /// <param name="next"></param>
    private void CreateTable(IEntityDescriptor descriptor, bool next = false)
    {
        using var con = Context.NewConnection();
        con.Open();

        var tableName = ResolveTableName(descriptor, next);

        //判断表是否存在，只有不存时会执行创建操作并会触发对应的创建前后事件
        if (Context.SchemaProvider.IsExistsTable(con.Database, tableName))
        {
            //更新列
            if (Options.UpdateColumn)
                UpdateColumn(descriptor, con);

            con.Close();
        }
        else
        {
            Options.BeforeCreateTable?.Invoke(Context, descriptor);

            var sql = GenerateCreateTableSql(descriptor, tableName);

            con.Execute(sql);

            con.Close();

            Options.AfterCreateTable?.Invoke(Context, descriptor);
        }
    }

    private string GenerateCreateTableSql(IEntityDescriptor descriptor, string tableName)
    {
        var columns = descriptor.Columns;
        var sql = new StringBuilder();
        sql.AppendFormat("CREATE TABLE {0}(", AppendQuote(tableName));

        for (int i = 0; i < columns.Count; i++)
        {
            var column = columns[i];

            sql.Append(GenerateColumnAddSql(column, descriptor));

            if (i < columns.Count - 1)
            {
                sql.Append(",");
            }
        }

        sql.Append(") ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;");

        return sql.ToString();
    }

    /// <summary>
    /// 更新列信息
    /// </summary>
    private void UpdateColumn(IEntityDescriptor descriptor, IDbConnection con)
    {
        var columns = Context.SchemaProvider.GetColumns(con.Database, descriptor.TableName);
        //保存删除后的列信息
        var cleanColumns = new List<ColumnSchema>();

        //删除列
        foreach (var column in columns)
        {
            //如果列名称、列类型或者可空配置不一样，则删除重建
            var deleted = descriptor.Columns.FirstOrDefault(m => m.Name.Equals(column.Name));
            if (deleted == null || CompareColumnInfo(deleted, column))
            {
                var deleteSql = $"ALTER TABLE {AppendQuote(descriptor.TableName)} DROP COLUMN {AppendQuote(column.Name)};";
                con.Execute(deleteSql);
            }
            else
            {
                cleanColumns.Add(column);
            }
        }

        //添加列
        foreach (var column in descriptor.Columns)
        {
            var add = cleanColumns.FirstOrDefault(m => m.Name.Equals(column.Name));
            if (add == null)
            {
                var addSql = $"ALTER TABLE {AppendQuote(descriptor.TableName)} ADD COLUMN {GenerateColumnAddSql(column, descriptor)}";

                con.Execute(addSql);
            }
        }
    }

    /// <summary>
    /// 比对列信息
    /// </summary>
    /// <returns></returns>
    private bool CompareColumnInfo(IColumnDescriptor descriptor, ColumnSchema schema)
    {
        //类型修改
        if (!descriptor.TypeName.EqualsIgnoreCase(schema.DataType))
            return true;

        switch (descriptor.TypeName)
        {
            case "CHAR":
                //MySql中使用CHAR(36)来保存GUID格式
                if (!descriptor.PropertyInfo.PropertyType.IsGuid() && descriptor.Length != schema.Length)
                    return true;
                break;
            case "VARCHAR":
                if (descriptor.Length != schema.Length)
                    return true;
                break;
            case "DECIMAL":
            case "DOUBLE":
            case "FLOAT":
                var precision = descriptor.Precision < 1 ? 18 : descriptor.Precision;
                var scale = descriptor.Scale < 1 ? 4 : descriptor.Scale;
                if (precision != schema.Precision || scale != schema.Scale)
                    return true;
                break;
        }

        //可空修改
        if (descriptor.Nullable != schema.IsNullable)
            return true;

        //说明修改
        if (descriptor.Description != null && !descriptor.Description.EqualsIgnoreCase(schema.Description))
            return true;

        //默认值修改
        if (descriptor.DefaultValue != null && !descriptor.DefaultValue.EqualsIgnoreCase(schema.DefaultValue))
            return true;

        return false;
    }

    private string GenerateColumnAddSql(IColumnDescriptor column, IEntityDescriptor descriptor)
    {
        var sql = new StringBuilder();
        sql.AppendFormat("{0} ", AppendQuote(column.Name));

        switch (column.TypeName)
        {
            case "CHAR":
                //MySql中使用CHAR(36)来保存GUID格式
                sql.AppendFormat("CHAR({0}) ", column.Length);
                break;
            case "VARCHAR":
                sql.AppendFormat("VARCHAR({0}) ", column.Length);
                break;
            case "DECIMAL":
            case "DOUBLE":
            case "FLOAT":
                var precision = column.Precision < 1 ? 18 : column.Precision;
                var scale = column.Scale < 1 ? 4 : column.Scale;
                sql.AppendFormat("{0}({1},{2}) ", column.TypeName, precision, scale);
                break;
            default:
                sql.AppendFormat("{0} ", column.TypeName);
                break;
        }
        if (column.IsPrimaryKey)
        {
            sql.Append("PRIMARY KEY ");

            if (descriptor.PrimaryKey.IsInt || descriptor.PrimaryKey.IsLong)
            {
                sql.Append("AUTO_INCREMENT ");
            }
        }

        if (!column.Nullable)
        {
            sql.Append("NOT NULL ");
        }

        if (!column.IsPrimaryKey && column.DefaultValue.NotNull())
        {
            sql.AppendFormat("DEFAULT {0}", column.DefaultValue);
        }

        if (column.Description.NotNull())
        {
            sql.AppendFormat(" COMMENT '{0}' ", column.Description);
        }

        return sql.ToString();
    }

    #endregion

    private string AppendQuote(string value)
    {
        return Context.Adapter.AppendQuote(value);
    }
}