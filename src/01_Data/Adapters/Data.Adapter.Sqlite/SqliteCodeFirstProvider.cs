using System.Linq;
using System.Text;
using Dapper;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Options;

namespace Mkh.Data.Adapter.Sqlite
{
    public class SqliteCodeFirstProvider : ICodeFirstProvider
    {
        private readonly CodeFirstOptions _options;
        private readonly IDbContext _context;

        public SqliteCodeFirstProvider(CodeFirstOptions options, IDbContext context)
        {
            _options = options;
            _context = context;
        }

        #region ==创建库==

        public void CreateDatabase()
        {
            //sqlite本身就包含自动创建数据库功能
        }

        #endregion

        #region ==创建表==

        public void CreateTable()
        {
            //创建表
            foreach (var descriptor in _context.EntityDescriptors.Where(m => m.AutoCreate))
            {
                using var con = _context.NewConnection();
                con.Open();

                //判断表是否存在，只有不存时会执行创建操作并会触发对应的创建前后事件
                if (_context.SchemaProvider.IsExistsTable(con.Database, descriptor.TableName))
                {
                    //更新列
                    if (_options.UpdateColumn)
                    {
                        //Sqlite不支持从一张表中删除列、添加列操作
                    }

                    con.Close();
                }
                else
                {
                    _options.BeforeCreateTable?.Invoke(_context, descriptor);

                    var sql = GenerateCreateTableSql(descriptor);

                    con.Execute(sql);
                    con.Close();

                    _options.BeforeCreateTable?.Invoke(_context, descriptor);
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
            return _context.Adapter.AppendQuote(value);
        }
    }
}
