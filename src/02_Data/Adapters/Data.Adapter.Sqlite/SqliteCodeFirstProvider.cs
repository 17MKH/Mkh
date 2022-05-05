using System.Linq;
using System.Text;
using Dapper;
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
    }

    #endregion

    #region ==创建表==

    public override void CreateTable()
    {
        //创建表
        foreach (var descriptor in Context.EntityDescriptors.Where(m => m.AutoCreate))
        {
            using var con = Context.NewConnection();
            con.Open();

            //判断表是否存在，只有不存时会执行创建操作并会触发对应的创建前后事件
            if (Context.SchemaProvider.IsExistsTable(con.Database, descriptor.TableName))
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

                var sql = GenerateCreateTableSql(descriptor);

                con.Execute(sql);
                con.Close();

                Options.AfterCreateTable?.Invoke(Context, descriptor);
            }
        }
    }

    private string GenerateCreateTableSql(IEntityDescriptor descriptor)
    {
        var columns = descriptor.Columns;
        var sql = new StringBuilder();
        sql.AppendFormat("CREATE TABLE {0}(", AppendQuote(descriptor.TableName));

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

        return sql.ToString();
    }

    #endregion

    private string AppendQuote(string value)
    {
        return Context.Adapter.AppendQuote(value);
    }
}