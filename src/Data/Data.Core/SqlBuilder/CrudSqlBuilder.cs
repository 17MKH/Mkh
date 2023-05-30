using System.Linq;
using System.Text;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Core.Descriptors;
using Mkh.Data.Core.Extensions;

namespace Mkh.Data.Core.SqlBuilder;

/// <summary>
/// 实体基本CRUD的SQL语句构造器
/// </summary>
internal class CrudSqlBuilder
{
    private readonly IEntityDescriptor _descriptor;
    private readonly EntitySqlDescriptor _sql;
    public CrudSqlBuilder(IEntityDescriptor descriptor)
    {
        _descriptor = descriptor;
        _sql = new EntitySqlDescriptor(_descriptor.DbContext.Adapter, _descriptor.TableName);
    }

    public EntitySqlDescriptor Build()
    {
        BuildInsertSql();
        BuildDeleteSql();
        BuildSoftDeleteSql();
        BuildUpdateSql();
        BuildQuerySql();
        BuildExistsSql();

        return _sql;
    }

    #region ==私有方法==

    /// <summary>
    /// 生成插入语句
    /// </summary>
    private void BuildInsertSql()
    {
        var sb = new StringBuilder();
        sb.Append("INSERT INTO {0} ");
        sb.Append("(");

        var valuesSql = new StringBuilder();
        var primaryKey = _descriptor.PrimaryKey;
        var dbAdapter = _descriptor.DbContext.Adapter;

        foreach (var col in _descriptor.Columns)
        {
            //排除自增主键
            if (col.IsPrimaryKey && (primaryKey.IsInt || primaryKey.IsLong))
                continue;

            dbAdapter.AppendQuote(sb, col.Name);
            sb.Append(",");

            dbAdapter.AppendParameter(valuesSql, col.PropertyName);

            //针对PostgreSQL数据库的json和jsonb类型字段的处理
            if (dbAdapter.Provider == DbProvider.PostgreSQL)
            {
                if (col.TypeName.EqualsIgnoreCase("jsonb"))
                {
                    valuesSql.Append("::jsonb");
                }
                else if (col.TypeName.EqualsIgnoreCase("json"))
                {
                    valuesSql.Append("::json");
                }
            }

            valuesSql.Append(",");
        }

        //删除最后一个","
        sb.Remove(sb.Length - 1, 1);

        sb.Append(") VALUES");

        //设置批量添加
        _sql.SetBulkAdd(sb.ToString());

        sb.Append("(");

        //删除最后一个","
        if (valuesSql.Length > 0)
            valuesSql.Remove(valuesSql.Length - 1, 1);

        sb.Append(valuesSql);
        sb.Append(")");

        if (dbAdapter.Provider != DbProvider.PostgreSQL)
        {
            sb.Append(";");
        }

        _sql.SetAdd(sb.ToString());
    }

    /// <summary>
    /// 设置删除语句
    /// </summary>
    private void BuildDeleteSql()
    {
        var deleteSql = "DELETE FROM {0} ";
        _sql.SetDelete(deleteSql);

        var primaryKey = _descriptor.PrimaryKey;
        if (!primaryKey.IsNo)
        {
            var dbAdapter = _descriptor.DbContext.Adapter;
            _sql.SetDeleteSingle($"{deleteSql} WHERE {dbAdapter.AppendQuote(primaryKey.ColumnName)}={dbAdapter.AppendParameter(primaryKey.PropertyName)};");
        }
    }

    /// <summary>
    /// 设置软删除
    /// </summary>
    private void BuildSoftDeleteSql()
    {
        if (!_descriptor.IsSoftDelete)
            return;

        var dbAdapter = _descriptor.DbContext.Adapter;
        var sb = new StringBuilder("UPDATE {0} SET ");
        sb.AppendFormat("{0}={1},", dbAdapter.AppendQuote(_descriptor.GetDeletedColumnName()), dbAdapter.BooleanTrueValue);
        sb.AppendFormat("{0}={1},", dbAdapter.AppendQuote(_descriptor.GetDeletedTimeColumnName()), dbAdapter.AppendParameter("DeletedTime"));
        sb.AppendFormat("{0}={1} ", dbAdapter.AppendQuote(_descriptor.GetDeletedByColumnName()), dbAdapter.AppendParameter("DeletedBy"));

        _sql.SetSoftDelete(sb.ToString());

        var primaryKey = _descriptor.PrimaryKey;
        sb.AppendFormat(" WHERE {0}={1};", dbAdapter.AppendQuote(primaryKey.ColumnName), dbAdapter.AppendParameter(primaryKey.PropertyName));
        _sql.SetSoftDeleteSingle(sb.ToString());
    }

    /// <summary>
    /// 设置更新语句
    /// </summary>
    private void BuildUpdateSql()
    {
        var sb = new StringBuilder();
        sb.Append("UPDATE {0} SET");

        _sql.SetUpdate(sb.ToString());

        var dbAdapter = _descriptor.DbContext.Adapter;
        var primaryKey = _descriptor.PrimaryKey;
        if (!primaryKey.IsNo)
        {
            var columns = _descriptor.Columns.Where(m => !m.IsPrimaryKey);

            foreach (var col in columns)
            {
                sb.AppendFormat("{0}={1}", dbAdapter.AppendQuote(col.Name), dbAdapter.AppendParameter(col.PropertyName));

                //针对PostgreSQL数据库的json和jsonb类型字段的处理
                if (dbAdapter.Provider == DbProvider.PostgreSQL)
                {
                    if (col.TypeName.EqualsIgnoreCase("jsonb"))
                    {
                        sb.Append("::jsonb");
                    }
                    else if (col.TypeName.EqualsIgnoreCase("json"))
                    {
                        sb.Append("::json");
                    }
                }

                sb.Append(",");
            }

            sb.Remove(sb.Length - 1, 1);

            sb.AppendFormat(" WHERE {0}={1};", dbAdapter.AppendQuote(primaryKey.ColumnName), dbAdapter.AppendParameter(primaryKey.PropertyName));

            _sql.SetUpdateSingle(sb.ToString());
        }
    }

    /// <summary>
    /// 设置查询语句
    /// </summary>
    private void BuildQuerySql()
    {
        var dbAdapter = _descriptor.DbContext.Adapter;
        var sb = new StringBuilder("SELECT ");
        for (var i = 0; i < _descriptor.Columns.Count; i++)
        {
            var col = _descriptor.Columns[i];
            sb.AppendFormat("{0} AS {1}", dbAdapter.AppendQuote(col.Name), dbAdapter.AppendQuote(col.PropertyName));

            if (i != _descriptor.Columns.Count - 1)
            {
                sb.Append(",");
            }
        }
        sb.Append(" FROM {0} ");

        var querySql = sb.ToString();
        var getSql = querySql;
        var getAndRowLockSql = querySql;
        var getAndNoLockSql = querySql;
        // SqlServer行锁
        if (dbAdapter.Provider == DbProvider.SqlServer)
        {
            getAndRowLockSql += " WITH (ROWLOCK, UPDLOCK) ";
            getAndNoLockSql += "WITH (NOLOCK) ";
        }

        var primaryKey = _descriptor.PrimaryKey;
        if (!primaryKey.IsNo)
        {
            var appendSql = $" WHERE {dbAdapter.AppendQuote(primaryKey.ColumnName)}={dbAdapter.AppendParameter(primaryKey.PropertyName)} ";
            getSql += appendSql;
            getAndRowLockSql += appendSql;
            getAndNoLockSql += appendSql;

            //过滤软删除
            if (_descriptor.IsSoftDelete)
            {
                appendSql = $" AND {dbAdapter.AppendQuote(_descriptor.GetDeletedColumnName())}={dbAdapter.BooleanFalseValue} ";
                getSql += appendSql;
                getAndRowLockSql += appendSql;
                getAndNoLockSql += appendSql;
            }

            //MySql和PostgreSQL行锁
            if (dbAdapter.Provider == DbProvider.MySql || dbAdapter.Provider == DbProvider.PostgreSQL)
            {
                getAndRowLockSql += " FOR UPDATE;";
            }
        }

        _sql.SetGet(getSql);
        _sql.SetGetAndRowLock(getAndRowLockSql);
        _sql.SetGetAndNoLock(getAndNoLockSql);
    }

    /// <summary>
    /// 设置是否存在语句
    /// </summary>
    /// <returns></returns>
    private void BuildExistsSql()
    {
        var primaryKey = _descriptor.PrimaryKey;
        //没有主键，无法使用该方法
        if (primaryKey.IsNo)
            return;

        var dbAdapter = _descriptor.DbContext.Adapter;
        var sql = $"SELECT 1 FROM {{0}} WHERE {dbAdapter.AppendQuote(primaryKey.ColumnName)}={dbAdapter.AppendParameter(primaryKey.PropertyName)}";
        if (_descriptor.IsSoftDelete)
        {
            sql += $" AND {dbAdapter.AppendQuote(_descriptor.GetDeletedColumnName())}={dbAdapter.BooleanFalseValue} ";
        }

        if (dbAdapter.Provider == DbProvider.MySql)
        {
            sql += " LIMIT 1";
        }

        _sql.SetExists(sql);
    }

    #endregion
}