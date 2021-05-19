using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Options;
using Mkh.Data.Abstractions.Schema;

namespace Mkh.Data.Adapter.MySql
{
    public class MySqlCodeFirstProvider : ICodeFirstProvider
    {
        private readonly CodeFirstOptions _options;
        private readonly IDbContext _context;

        public MySqlCodeFirstProvider(CodeFirstOptions options, IDbContext context)
        {
            _options = options;
            _context = context;
        }

        #region ==创建库==

        public void CreateDatabase()
        {
            var con = _context.NewConnection();

            //数据库名称
            var databaseName = con.Database;
            /*
            * 创建数据库需要当前连接字符串使用的账户拥有对应的权限，并且需要使用mysql数据库
            */
            var connString = con.ConnectionString.Replace(databaseName, "mysql");
            using var con1 = _context.NewConnection(connString);
            var schemaProvider = new MySqlSchemaProvider(con1);
            var databaseExists = schemaProvider.IsExistsDatabase(databaseName);
            if (databaseExists)
            {
                con.Close();
                return;
            }

            //创建前事件
            _options.BeforeCreateDatabase?.Invoke(_context);

            //创建数据库
            schemaProvider.CreateDatabase(databaseName);

            con1.Close();

            //创建后事件
            _options.BeforeCreateDatabase?.Invoke(_context);
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
                        UpdateColumn(descriptor, con);

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
            var columns = _context.SchemaProvider.GetColumns(con.Database, descriptor.TableName);
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
            if (!descriptor.Description.EqualsIgnoreCase(schema.Description))
                return true;

            //默认值修改
            if (!descriptor.DefaultValue.EqualsIgnoreCase(schema.DefaultValue))
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
                    sql.AppendFormat("CHAR({0}) ", column.PropertyInfo.PropertyType.IsGuid() ? 36 : column.Length);
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
            return _context.Adapter.AppendQuote(value);
        }
    }
}
