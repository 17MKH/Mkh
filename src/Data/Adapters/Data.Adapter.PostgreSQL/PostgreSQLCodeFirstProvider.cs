using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Options;
using Mkh.Data.Abstractions.Schema;
using Mkh.Data.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using static Npgsql.Replication.PgOutput.Messages.RelationMessage;
using static System.Formats.Asn1.AsnWriter;

namespace Mkh.Data.Adapter.PostgreSQL
{
    internal class PostgreSQLCodeFirstProvider : CodeFirstProviderAbstract
    {
        public PostgreSQLCodeFirstProvider(CodeFirstOptions options, IDbContext context, IServiceCollection service) : base(options, context, service)
        {
        }

        public override void CreateDatabase()
        {
            var con = Context.NewConnection();

            //数据库名称
            var databaseName = con.Database;


            //判断数据库是否存在
            var schemaProvider = new PostgreSQLSchemaProvider(con);
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

            //创建后事件
            Options.AfterCreateDatabase?.Invoke(Context);
        }

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
                    UpdateColumn(descriptor, con, tableName);

                con.Close();
            }
            else
            {
                Options.BeforeCreateTable?.Invoke(Context, descriptor);

                var (sql, commentSql) = GenerateCreateTableSql(descriptor, tableName);

                con.Execute(sql);
                if (!string.IsNullOrWhiteSpace(commentSql))
                {
                    con.Execute(commentSql);
                }

                con.Close();

                Options.AfterCreateTable?.Invoke(Context, descriptor);
            }
        }

        (string sql, string commentSql) GenerateCreateTableSql(IEntityDescriptor descriptor, string tableName)
        {
            var columns = descriptor.Columns;
            var sql = new StringBuilder();
            var sqlComment = new StringBuilder(128);
            sql.AppendFormat("CREATE TABLE {0}(", AppendQuote(tableName));

            for (int i = 0; i < columns.Count; i++)
            {
                var column = columns[i];

                sql.Append(GenerateColumnAddSql(column, tableName));

                //生成字段备注
                if (column.Description.NotNull())
                {
                    sqlComment.AppendFormat($"COMMENT ON COLUMN {this.AppendQuote(tableName)}.{this.AppendQuote(column.Name)} is '{column.Description}'; ");
                }

                if (i < columns.Count - 1)
                {
                    sql.Append(",");
                }
            }

            sql.Append(")");


            return (sql.ToString(), sqlComment.ToString());
        }

        /// <summary>
        /// 更新列信息
        /// </summary>
        private void UpdateColumn(IEntityDescriptor descriptor, IDbConnection con, string tableName)
        {
            using var trans = con.BeginTransaction();

            var columns = Context.SchemaProvider.GetColumns(con.Database, tableName);
            var commentSql = new StringBuilder(128);

            try
            {
                //对比列，添加或修改列
                foreach (var column in descriptor.Columns)
                {
                    var existsCol = columns.FirstOrDefault(m => m.Name.Equals(column.Name, StringComparison.CurrentCultureIgnoreCase));
                    if (existsCol == null)
                    {
                        var addSql = $"ALTER TABLE {AppendQuote(tableName)} ADD COLUMN {GenerateColumnAddSql(column, tableName)}";
                        //生成字段备注
                        if (column.Description.NotNull())
                        {
                            commentSql.AppendFormat($"COMMENT ON COLUMN {this.AppendQuote(tableName)}.{this.AppendQuote(column.Name)} is '{column.Description}'; ");
                        }
                        con.Execute(addSql, transaction: trans);
                    }
                    else
                    {
                        CompareColumn(column, existsCol, tableName, con, trans);

                        if (string.IsNullOrWhiteSpace(column.Description) && !string.IsNullOrWhiteSpace(existsCol.Description))
                        {
                            commentSql.AppendFormat($"COMMENT ON COLUMN {this.AppendQuote(tableName)}.{this.AppendQuote(existsCol.Name)} is '{existsCol.Description}'; ");
                        }
                    }
                }

                if (commentSql.Length > 0)
                {
                    con.Execute(commentSql.ToString(), transaction: trans);
                }

                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
            
        }

        /// <summary>
        /// 对比字段变化，执行修改sql
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="colSchema"></param>
        /// <param name="tableName"></param>
        /// <param name="con"></param>
        /// <param name="trans"></param>
        void CompareColumn(IColumnDescriptor descriptor, ColumnSchema colSchema, string tableName,  IDbConnection con, IDbTransaction trans)
        {
            //类型修改
            if (!descriptor.TypeName.EqualsIgnoreCase(colSchema.DataType))
                return ;

            switch (descriptor.TypeName?.ToUpper())
            {
                case "CHAR":
                    if (descriptor.Length != colSchema.Length)
                        con.Execute($"ALTER TABLE {this.AppendQuote(tableName)} ALTER COLUMN {this.AppendQuote(descriptor.Name)} TYPE character({descriptor.Length});", transaction: trans);
                        break;
                case "VARCHAR":
                    if (descriptor.Length != colSchema.Length)
                        con.Execute($"ALTER TABLE {this.AppendQuote(tableName)} ALTER COLUMN {this.AppendQuote(descriptor.Name)} TYPE VARCHAR({descriptor.Length});", transaction: trans);
                    break;
                case "NUMERIC":
                    var precision = descriptor.Precision < 1 ? 18 : descriptor.Precision;
                    var scale = descriptor.Scale < 1 ? 4 : descriptor.Scale;
                    if (precision != colSchema.Precision || scale != colSchema.Scale)
                        con.Execute($"ALTER TABLE {this.AppendQuote(tableName)} ALTER COLUMN {this.AppendQuote(descriptor.Name)} TYPE NUMERIC({precision}, {scale});", transaction: trans);
                    break;
            }

            
        }


        private string GenerateColumnAddSql(IColumnDescriptor column, string tableName)
        {
            var sql = new StringBuilder();
            sql.AppendFormat("{0} ", AppendQuote(column.Name));

            switch (column.TypeName?.ToUpper())
            {
                case "CHAR":
                    sql.AppendFormat("CHARACTER({0}) ", column.Length);
                    break;
                case "VARCHAR":
                    sql.AppendFormat("VARCHAR({0}) ", column.Length);
                    break;
                case "DECIMAL":
                //case "DOUBLE":
                //case "FLOAT":
                    var precision = column.Precision < 1 ? 18 : column.Precision;
                    var scale = column.Scale < 1 ? 4 : column.Scale;
                    sql.AppendFormat("{0}({1},{2}) ", column.TypeName, precision, scale);
                    break;
                case "INT2":
                    if (column.IsPrimaryKey)
                    {
                        sql.AppendFormat("SMALLSERIAL ");
                    }
                    else
                    {
                        sql.AppendFormat("{0} ", column.TypeName);
                    }
                    break;
                case "INT4":
                case "INTEGER":
                    if (column.IsPrimaryKey)
                    {
                        sql.AppendFormat("SERIAL ");
                    }
                    else
                    {
                        sql.AppendFormat("{0} ", column.TypeName);
                    }
                    break;
                case "INT8":
                case "BIGINT":
                    if (column.IsPrimaryKey)
                    {
                        sql.AppendFormat("BIGSERIAL ");
                    }
                    else
                    {
                        sql.AppendFormat("{0} ", column.TypeName);
                    }
                    break;
                default:
                    sql.AppendFormat("{0} ", column.TypeName);
                    break;
            }
            if (column.IsPrimaryKey)
            {
                sql.Append("PRIMARY KEY ");
            }

            if (!column.Nullable)
            {
                sql.Append("NOT NULL ");
            }

            if (!column.IsPrimaryKey && column.DefaultValue.NotNull())
            {
                sql.AppendFormat("DEFAULT {0}", column.DefaultValue);
            }

            //if (column.Description.NotNull())
            //{
            //    commentSql.AppendFormat($"COMMENT ON COLUMN {this.AppendQuote(tableName)}.{this.AppendQuote(column.Name)} is '{column.Description}'; ");
            //}

            return sql.ToString();
        }

        private string AppendQuote(string value)
        {
            return Context.Adapter.AppendQuote(value);
        }
    }
}
