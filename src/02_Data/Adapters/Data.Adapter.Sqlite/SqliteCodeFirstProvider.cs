using System.IO;
using System.Linq;
using System.Text;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Options;
using Mkh.Data.Core;

namespace Mkh.Data.Adapter.Sqlite;

public class SqliteCodeFirstProvider : CodeFirstProviderAbstract
{
    public SqliteCodeFirstProvider(CodeFirstOptions options, IDbContext context, IServiceCollection service) : base(options, context, service)
    {
    }

    #region ==创建库==

    public override void CreateDatabase()
    {
        var builder = new SqliteConnectionStringBuilder(Context.Options.ConnectionString);
        if (File.Exists(builder.ConnectionString))
        {
            return;
        }

        Options.BeforeCreateDatabase?.Invoke(Context);

        //SQLite会自动创建数据库文件
        using var con = Context.NewConnection();
        con.Open();
        con.Close();

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
            {
                Context.Logger.Write("SQLite CreateTable", "SQLite不支持更新列功能，您需要手动更新");
            }

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

            sql.Append(GetColumnAddSql(column, descriptor));

            if (i < columns.Count - 1)
            {
                sql.Append(",");
            }
        }

        sql.Append(");");

        return sql.ToString();
    }

    private string GetColumnAddSql(IColumnDescriptor column, IEntityDescriptor descriptor)
    {
        var sql = new StringBuilder();
        sql.AppendFormat("{0} ", AppendQuote(column.Name));

        switch (column.TypeName)
        {
            case "decimal":
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
                sql.Append("AUTOINCREMENT ");
            }
        }
        if (!column.IsPrimaryKey && column.DefaultValue.NotNull())
        {
            sql.AppendFormat("DEFAULT {0}", column.DefaultValue);
        }

        if (!column.Nullable)
        {
            sql.Append(" NOT NULL ");
        }

        //不区分大小写
        sql.Append(" COLLATE NOCASE ");

        return sql.ToString();
    }

    #endregion

    private string AppendQuote(string value)
    {
        return Context.Adapter.AppendQuote(value);
    }
}