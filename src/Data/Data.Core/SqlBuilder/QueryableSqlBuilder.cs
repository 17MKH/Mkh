using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Pagination;
using Mkh.Data.Abstractions.Queryable;
using Mkh.Data.Core.Extensions;
using Mkh.Data.Core.Internal;
using Mkh.Data.Core.Internal.QueryStructure;

namespace Mkh.Data.Core.SqlBuilder;

/// <summary>
/// 查询SQL生成器
/// </summary>
public class QueryableSqlBuilder
{
    public QueryBody QueryBody => _queryBody;

    public bool IsSingleEntity => _queryBody.Joins.Count < 2;

    private readonly QueryBody _queryBody;
    private readonly IDbAdapter _dbAdapter;
    private readonly IDbContext _dbContext;

    public QueryableSqlBuilder(QueryBody queryBody)
    {
        _queryBody = queryBody;
        _dbContext = queryBody.Repository.DbContext;
        _dbAdapter = _dbContext.Adapter;
    }

    #region ==BuildListSql==

    /// <summary>
    /// 生成列表语句
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public string BuildListSql(out IQueryParameters parameters)
    {
        parameters = new QueryParameters();
        return BuildListSql(parameters);
    }

    /// <summary>
    /// 生成列表语句
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public string BuildListSql(IQueryParameters parameters)
    {
        var sqlBuilder = new StringBuilder();
        sqlBuilder.Append("SELECT ");
        ResolveSelect(sqlBuilder);
        sqlBuilder.Append(" FROM ");
        ExpressionResolver.ResolveFrom(_queryBody, sqlBuilder, parameters);
        sqlBuilder.Append(" ");
        ExpressionResolver.ResolveWhere(_queryBody, sqlBuilder, parameters);
        ResolveSort(sqlBuilder);

        return sqlBuilder.ToString();
    }

    /// <summary>
    /// 生成列表语句，不使用参数化
    /// </summary>
    /// <returns></returns>
    public string BuildListSqlNotUseParameters()
    {
        _queryBody.UseParameters = false;
        var sql = BuildListSql(out _);
        _queryBody.UseParameters = true;
        return sql;
    }

    #endregion

    #region ==BuildFirstSql==

    /// <summary>
    /// 生成获取第一条记录语句
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public string BuildFirstSql(out IQueryParameters parameters)
    {
        parameters = new QueryParameters();
        return BuildFirstSql(parameters);
    }

    /// <summary>
    /// 生成获取第一条记录语句
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public string BuildFirstSql(IQueryParameters parameters)
    {
        var select = ResolveSelect();
        var from = ExpressionResolver.ResolveFrom(_queryBody, parameters);
        var where = ExpressionResolver.ResolveWhere(_queryBody, parameters);
        var sort = ResolveSort();

        return _dbAdapter.GenerateFirstSql(select, from, where, sort);
    }

    /// <summary>
    /// 生成获取第一条记录语句，并且不使用参数化
    /// </summary>
    /// <returns></returns>
    public string BuildFirstSqlNotUserParameters()
    {
        _queryBody.UseParameters = false;
        var sql = BuildFirstSql(out _);
        _queryBody.UseParameters = true;
        return sql;
    }

    #endregion

    #region ==BuildPaginationSql==

    /// <summary>
    /// 生成分页查询语句
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public string BuildPaginationSql(out IQueryParameters parameters)
    {
        parameters = new QueryParameters();
        return BuildPaginationSql(parameters);
    }

    /// <summary>
    /// 生成分页查询语句
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public string BuildPaginationSql(IQueryParameters parameters)
    {
        var select = ResolveSelect();
        var from = ExpressionResolver.ResolveFrom(_queryBody, parameters);
        var where = ExpressionResolver.ResolveWhere(_queryBody, parameters);
        var sort = ResolveSort();

        return _dbAdapter.GeneratePagingSql(select, from, where, sort, _queryBody.Skip, _queryBody.Take);
    }

    /// <summary>
    /// 生成分页查询语句，并且不使用参数化
    /// </summary>
    /// <returns></returns>
    public string BuildPaginationSqlNotUseParameters()
    {
        _queryBody.UseParameters = false;
        var sql = BuildPaginationSql(out _);
        _queryBody.UseParameters = true;
        return sql;
    }

    #endregion

    #region ==BuildCountSql==

    public string BuildCountSql(out IQueryParameters parameters)
    {
        parameters = new QueryParameters();
        return BuildCountSql(parameters);
    }

    /// <summary>
    /// 生成数量查询SQL
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public string BuildCountSql(IQueryParameters parameters)
    {
        var sqlBuilder = new StringBuilder("SELECT COUNT(*) FROM ");

        ExpressionResolver.ResolveFrom(_queryBody, sqlBuilder, parameters);

        ExpressionResolver.ResolveWhere(_queryBody, sqlBuilder, parameters);

        var sql = sqlBuilder.ToString();

        return sql;
    }

    /// <summary>
    /// 生成数量查询SQL，并且不使用参数化
    /// </summary>
    /// <returns></returns>
    public string BuildCountSqlNotUseParameters()
    {
        _queryBody.UseParameters = false;
        var sql = BuildCountSql(out _);
        _queryBody.UseParameters = true;
        return sql;
    }

    #endregion

    #region ==BuildExistsSql==

    /// <summary>
    /// 生成判断存在语句
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public string BuildExistsSql(out IQueryParameters parameters)
    {
        parameters = new QueryParameters();
        return BuildExistsSql(parameters);
    }

    /// <summary>
    /// 生成判断存在语句
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public string BuildExistsSql(IQueryParameters parameters)
    {
        var select = "1";
        var from = ExpressionResolver.ResolveFrom(_queryBody, parameters);
        var where = ExpressionResolver.ResolveWhere(_queryBody, parameters);
        var sort = ResolveSort();

        var sql = _dbAdapter.GenerateFirstSql(select, from, where, sort);

        return sql;
    }

    /// <summary>
    /// 生成判断存在语句，并且不使用参数化
    /// </summary>
    /// <returns></returns>
    public string BuildExistsSqlNotUseParameters()
    {
        _queryBody.UseParameters = false;
        var sql = BuildExistsSql(out _);
        _queryBody.UseParameters = true;
        return sql;
    }

    #endregion

    #region ==BuildFunctionSql==

    /// <summary>
    /// 生成函数SQL
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public string BuildFunctionSql(out IQueryParameters parameters)
    {
        parameters = new QueryParameters();
        return BuildFunctionSql(parameters);
    }

    /// <summary>
    /// 生成函数SQL
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public string BuildFunctionSql(IQueryParameters parameters)
    {

        var sqlBuilder = new StringBuilder();
        sqlBuilder.Append("SELECT ");

        Expression memberExp;
        if (_queryBody.Select.FunctionExpression.Body.NodeType == ExpressionType.Convert)
        {
            memberExp = (_queryBody.Select.FunctionExpression.Body as UnaryExpression)!.Operand;
        }
        else
        {
            memberExp = _queryBody.Select.FunctionExpression.Body;
        }

        var columnName = _queryBody.GetColumnName(memberExp);

        sqlBuilder.Append(_dbAdapter.FunctionMapper(_queryBody.Select.FunctionName, columnName));

        sqlBuilder.Append(" FROM ");
        ExpressionResolver.ResolveFrom(_queryBody, sqlBuilder, parameters);
        sqlBuilder.Append(" ");
        ExpressionResolver.ResolveWhere(_queryBody, sqlBuilder, parameters);

        return sqlBuilder.ToString();
    }

    /// <summary>
    /// 生成函数SQL，并且不使用参数化
    /// </summary>
    /// <returns></returns>
    public string BuildFunctionSqlNotUseParameters()
    {
        _queryBody.UseParameters = false;
        var sql = BuildFunctionSql(out _);
        _queryBody.UseParameters = true;
        return sql;
    }

    #endregion

    #region ==BuildUpdateSql==

    /// <summary>
    /// 生成更新SQL
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public string BuildUpdateSql(out IQueryParameters parameters)
    {
        parameters = new QueryParameters();
        return BuildUpdateSql(parameters);
    }

    /// <summary>
    /// 生成更新SQL
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public string BuildUpdateSql(IQueryParameters parameters)
    {
        var tableName = _queryBody.Joins.First().TableName;
        Check.NotNull(tableName, nameof(tableName), "未指定更新表");

        var sqlBuilder = new StringBuilder();

        //更新语句优先
        var updateSql = _queryBody.Update.Sql.NotNull() ? _queryBody.Update.Sql : ExpressionResolver.Resolve(_queryBody, _queryBody.Update.Lambda, parameters);
        Check.NotNull(updateSql, nameof(updateSql), "生成更新sql异常");

        sqlBuilder.AppendFormat("UPDATE {0} SET ", _dbAdapter.AppendQuote(tableName));
        sqlBuilder.Append(updateSql);

        SetUpdateInfo(sqlBuilder, parameters);

        var whereSql = ExpressionResolver.ResolveWhere(_queryBody, parameters);
        Check.NotNull(whereSql, nameof(whereSql), "批量更新必须指定条件，防止人为失误误操作");
        sqlBuilder.AppendFormat(" {0};", whereSql);

        return sqlBuilder.ToString();
    }

    /// <summary>
    /// 生成更新SQL，并且不使用参数化
    /// </summary>
    /// <returns></returns>
    public string BuildUpdateSqlNotUseParameters()
    {
        _queryBody.UseParameters = false;
        var sql = BuildUpdateSql(out _);
        _queryBody.UseParameters = true;
        return sql;
    }

    /// <summary>
    /// 设置更新信息
    /// </summary>
    /// <param name="sqlBuilder"></param>
    /// <param name="parameters"></param>
    private void SetUpdateInfo(StringBuilder sqlBuilder, IQueryParameters parameters)
    {
        var descriptor = _queryBody.Joins.First().EntityDescriptor;
        if (descriptor.IsEntityBase)
        {
            if (_queryBody.UseParameters)
            {
                var p1 = parameters.Add(_dbContext.AccountResolver.AccountId);
                sqlBuilder.AppendFormat(",{0} = @{1}", _dbAdapter.AppendQuote(descriptor.GetModifiedByColumnName()), p1);
                var p2 = parameters.Add(_dbContext.AccountResolver.AccountName);
                sqlBuilder.AppendFormat(",{0} = @{1}", _dbAdapter.AppendQuote(descriptor.GetModifierColumnName()), p2);
                var p3 = parameters.Add(DateTime.Now);
                sqlBuilder.AppendFormat(",{0} = @{1}", _dbAdapter.AppendQuote(descriptor.GetModifiedTimeColumnName()), p3);
            }
            else
            {
                sqlBuilder.AppendFormat(",{0} = ", _dbAdapter.AppendQuote(descriptor.GetModifiedByColumnName()));
                ExpressionResolver.AppendValue(_queryBody, _dbContext.AccountResolver.AccountId, sqlBuilder, parameters);
                sqlBuilder.AppendFormat(",{0} = ", _dbAdapter.AppendQuote(descriptor.GetModifierColumnName()));
                ExpressionResolver.AppendValue(_queryBody, _dbContext.AccountResolver.AccountName, sqlBuilder, parameters);
                sqlBuilder.AppendFormat(",{0} = ", _dbAdapter.AppendQuote(descriptor.GetModifiedTimeColumnName()));
                ExpressionResolver.AppendValue(_queryBody, DateTime.Now, sqlBuilder, parameters);
            }
        }
    }

    #endregion

    #region ==BuildDeleteSql==

    /// <summary>
    /// 生成删除SQL
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public string BuildDeleteSql(out IQueryParameters parameters)
    {
        parameters = new QueryParameters();
        return BuildDeleteSql(parameters);
    }

    /// <summary>
    /// 生成删除SQL
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public string BuildDeleteSql(IQueryParameters parameters)
    {
        var tableName = _queryBody.Joins.First().TableName;
        Check.NotNull(tableName, nameof(tableName), "未指定更新表");

        var sqlBuilder = new StringBuilder();

        sqlBuilder.AppendFormat("DELETE FROM {0} ", _dbAdapter.AppendQuote(tableName));

        var whereSql = ExpressionResolver.ResolveWhere(_queryBody, parameters);
        Check.NotNull(whereSql, nameof(whereSql), "生成条件sql异常，删除必须指定条件，防止误操作");
        sqlBuilder.AppendFormat(" {0}", whereSql);

        return sqlBuilder.ToString();
    }

    /// <summary>
    /// 生成删除SQL，并且不使用参数化
    /// </summary>
    /// <returns></returns>
    public string BuildDeleteSqlNotUseParameters()
    {
        _queryBody.UseParameters = false;
        var sql = BuildDeleteSql(out _);
        _queryBody.UseParameters = true;
        return sql;
    }

    #endregion

    #region ==BuildSoftDeleteSql==

    /// <summary>
    /// 生成软删除SQL
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public string BuildSoftDeleteSql(out IQueryParameters parameters)
    {
        parameters = new QueryParameters();
        return BuildSoftDeleteSql(parameters);
    }

    /// <summary>
    /// 生成软删除SQL
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public string BuildSoftDeleteSql(IQueryParameters parameters)
    {
        var entityDescriptor = _queryBody.Joins.First().EntityDescriptor;
        if (!entityDescriptor.IsSoftDelete)
            throw new Exception("当前实体未实现软删除功能，无法调用该方法");

        var tableName = _queryBody.Joins.First().TableName;
        Check.NotNull(tableName, nameof(tableName), "未指定更新表");

        var deletedColumnName = entityDescriptor.GetDeletedColumnName();
        var deletedTimeColumnName = entityDescriptor.GetDeletedTimeColumnName();
        var deletedByColumnName = entityDescriptor.GetDeletedByColumnName();
        var deleterColumnName = entityDescriptor.GetDeleterColumnName();

        var sqlBuilder = new StringBuilder($"UPDATE {_dbAdapter.AppendQuote(tableName)} SET ");
        sqlBuilder.AppendFormat("{0} = {1},", _dbAdapter.AppendQuote(deletedColumnName), _dbAdapter.BooleanTrueValue);
        sqlBuilder.AppendFormat("{0} = ", _dbAdapter.AppendQuote(deletedTimeColumnName));
        ExpressionResolver.AppendValue(_queryBody, DateTime.Now, sqlBuilder, parameters);
        sqlBuilder.AppendFormat(",{0} = ", _dbAdapter.AppendQuote(deletedByColumnName));
        ExpressionResolver.AppendValue(_queryBody, _dbContext.AccountResolver.AccountId, sqlBuilder, parameters);
        sqlBuilder.AppendFormat(",{0} = ", _dbAdapter.AppendQuote(deleterColumnName));
        ExpressionResolver.AppendValue(_queryBody, _dbContext.AccountResolver.AccountName, sqlBuilder, parameters);

        var whereSql = ExpressionResolver.ResolveWhere(_queryBody, parameters);
        Check.NotNull(whereSql, nameof(whereSql), "生成条件sql异常");
        sqlBuilder.AppendFormat(" {0}", whereSql);

        return sqlBuilder.ToString();
    }

    /// <summary>
    /// 生成软删除SQL，并且不使用参数化
    /// </summary>
    /// <returns></returns>
    public string BuildSoftDeleteSqlNotUseParameters()
    {
        _queryBody.UseParameters = false;
        var sql = BuildSoftDeleteSql(out _);
        _queryBody.UseParameters = true;
        return sql;
    }

    #endregion

    #region ==解析查询列==

    public string ResolveSelect()
    {
        var sqlBuilder = new StringBuilder();
        ResolveSelect(sqlBuilder);
        return sqlBuilder.ToString();
    }

    /// <summary>
    /// 解析查询列
    /// </summary>
    /// <returns></returns>
    public void ResolveSelect(StringBuilder sqlBuilder)
    {
        var select = _queryBody.Select;

        //先解析出要排除的列
        var excludeColumns = ResolveSelectExcludeColumns();

        if (select == null || select.Mode == QuerySelectMode.UnKnown)
        {
            //解析所有实体
            ResolveSelectForEntity(sqlBuilder, 0, excludeColumns);
        }
        else if (select.Mode == QuerySelectMode.Sql)
        {
            //SQL语句
            sqlBuilder.Append(select.Sql);
        }
        else if (select.Mode == QuerySelectMode.Lambda)
        {
            //表达式
            var exp = select.Include.Body;
            switch (exp.NodeType)
            {
                //返回的整个实体
                case ExpressionType.Parameter:
                    ResolveSelectForEntity(sqlBuilder, 0, excludeColumns);
                    break;
                //返回的某个列
                case ExpressionType.MemberAccess:
                    ResolveSelectForMember(sqlBuilder, exp as MemberExpression, select.Include, excludeColumns);
                    if (sqlBuilder.Length > 0 && sqlBuilder[^1] == ',')
                    {
                        sqlBuilder.Remove(sqlBuilder.Length - 1, 1);
                    }
                    break;
                //自定义的返回对象
                case ExpressionType.New:
                    ResolveSelectForNew(sqlBuilder, exp as NewExpression, select.Include, excludeColumns);
                    break;
            }
        }

        //移除末尾的逗号
        if (sqlBuilder.Length > 0 && sqlBuilder[^1] == ',')
        {
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1);
        }
    }

    /// <summary>
    /// 解析排除列的名称列表
    /// </summary>
    /// <returns></returns>
    public List<IColumnDescriptor> ResolveSelectExcludeColumns()
    {
        if (_queryBody.Select != null && _queryBody.Select.Exclude != null)
        {
            var lambda = _queryBody.Select.Exclude;
            var body = lambda.Body;
            //整个实体
            if (body.NodeType == ExpressionType.Parameter)
            {
                throw new ArgumentException("不能排除整个实体的列");
            }

            var list = new List<IColumnDescriptor>();

            //返回的单个列
            if (body.NodeType == ExpressionType.MemberAccess)
            {
                var col = _queryBody.GetJoin(body as MemberExpression).Item2;
                if (col != null)
                    list.Add(col);

                return list;
            }

            //自定义的返回对象
            if (body.NodeType == ExpressionType.New)
            {
                var newExp = body as NewExpression;
                for (var i = 0; i < newExp!.Arguments.Count; i++)
                {
                    var arg = newExp.Arguments[i];
                    //实体
                    if (arg.NodeType == ExpressionType.Parameter)
                    {
                        throw new ArgumentException("不能排除整个实体");
                    }

                    //成员
                    if (arg.NodeType == ExpressionType.MemberAccess)
                    {
                        var col = _queryBody.GetJoin(arg as MemberExpression).Item2;
                        if (col != null)
                            list.Add(col);
                    }
                }
            }

            return list;
        }

        return null;
    }

    /// <summary>
    /// 解析查询列中的整个实体
    /// </summary>
    /// <param name="sqlBuilder"></param>
    /// <param name="index">多表连接时实体的下标</param>
    /// <param name="excludeColumns">排除列</param>
    public void ResolveSelectForEntity(StringBuilder sqlBuilder, int index = 0, List<IColumnDescriptor> excludeColumns = null)
    {
        var join = _queryBody.Joins[index];

        foreach (var col in join.EntityDescriptor.Columns)
        {
            if (excludeColumns != null && excludeColumns.Any(m => m == col))
                continue;

            //单个实体时不需要别名
            sqlBuilder.Append(IsSingleEntity ? $"{_dbAdapter.AppendQuote(col.Name)}" : $"{join.Alias}.{_dbAdapter.AppendQuote(col.Name)}");

            sqlBuilder.AppendFormat(" AS {0},", _dbAdapter.AppendQuote(col.PropertyInfo.Name));
        }
    }

    /// <summary>
    /// 解析查询列中的自定义类型
    /// </summary>
    /// <param name="sqlBuilder"></param>
    /// <param name="newExp"></param>
    /// <param name="fullLambda"></param>
    /// <param name="excludeColumns"></param>
    public void ResolveSelectForNew(StringBuilder sqlBuilder, NewExpression newExp, LambdaExpression fullLambda, List<IColumnDescriptor> excludeColumns = null)
    {
        for (var i = 0; i < newExp.Arguments.Count; i++)
        {
            var arg = newExp.Arguments[i];
            var alias = newExp.Members![i].Name;
            //成员
            if (arg.NodeType == ExpressionType.MemberAccess)
            {
                ResolveSelectForMember(sqlBuilder, arg as MemberExpression, fullLambda, excludeColumns, alias);
                continue;
            }
            //实体
            if (arg.NodeType == ExpressionType.Parameter && arg is ParameterExpression parameterExp)
            {
                ResolveSelectForEntity(sqlBuilder, fullLambda.Parameters.IndexOf(parameterExp), excludeColumns);
                continue;
            }
            //方法
            if (arg.NodeType == ExpressionType.Call && arg is MethodCallExpression callExp)
            {
                var columnName = _queryBody.GetColumnName(callExp!.Object);
                var args = ExpressionResolver.Arguments2Object(callExp.Arguments);
                sqlBuilder.AppendFormat("{0} AS {1},", _dbAdapter.FunctionMapper(callExp.Method.Name, columnName, callExp.Object!.Type, args), _dbAdapter.AppendQuote(alias));
            }
        }
    }

    public void ResolveSelectForMember(StringBuilder sqlBuilder, MemberExpression memberExp, LambdaExpression fullLambda, List<IColumnDescriptor> excludeCols, string alias = null)
    {
        alias ??= memberExp.Member.Name;
        string columnName;
        if (DbConstants.ENTITY_INTERFACE_TYPE.IsImplementType(memberExp.Type))
        {
            var index = _queryBody.Joins.FindIndex(m => m.EntityDescriptor.EntityType == memberExp.Type);
            ResolveSelectForEntity(sqlBuilder, index, excludeCols);
        }
        else if (memberExp.Expression!.Type.IsString())
        {
            columnName = _queryBody.GetColumnName(memberExp);
            sqlBuilder.AppendFormat("{0} AS {1},", _queryBody.DbAdapter.FunctionMapper(memberExp.Member.Name, columnName), _dbAdapter.AppendQuote(alias));
        }
        else if (DbConstants.ENTITY_INTERFACE_TYPE.IsImplementType(memberExp.Expression.Type))
        {
            var join = _queryBody.GetJoin(memberExp);
            if (excludeCols != null && excludeCols.Any(m => m == join.Item2))
                return;

            columnName = _queryBody.GetColumnName(join.Item1, join.Item2);
            sqlBuilder.AppendFormat("{0} AS {1},", columnName, _dbAdapter.AppendQuote(alias));
        }
    }

    #endregion

    #region ==解析排序==

    /// <summary>
    /// 解析排序
    /// </summary>
    /// <returns></returns>
    public string ResolveSort()
    {
        if (_queryBody.Sorts.IsNullOrEmpty())
            return string.Empty;

        var sqlBuilder = new StringBuilder();
        ResolveSort(sqlBuilder);
        return sqlBuilder.ToString();
    }

    public void ResolveSort(StringBuilder sqlBuilder)
    {
        if (_queryBody.Sorts.IsNullOrEmpty())
        {
            #region ==SqlServer分页需要指定排序==

            //SqlServer分页需要指定排序，此处判断是否有主键，有主键默认按照主键排序
            if (_queryBody.Take > 0 && _dbAdapter.Provider == DbProvider.SqlServer)
            {
                var first = _queryBody.Joins.First();
                if (first.EntityDescriptor.PrimaryKey.IsNo)
                {
                    throw new Exception("SqlServer数据库没有主键的表需要指定排序字段才可以分页查询");
                }

                _queryBody.Sorts = new List<QuerySort>
                {
                    new QuerySort
                    {
                        Mode = QuerySortMode.Sql,
                        Sql = _queryBody.Joins.Count > 1 ? $"{_dbAdapter.AppendQuote(first.Alias)}.{_dbAdapter.AppendQuote(first.EntityDescriptor.PrimaryKey.ColumnName)}" : first.EntityDescriptor.PrimaryKey.ColumnName,
                        Type = SortType.Desc
                    }
                };
            }
            else
            {
                return;
            }

            #endregion
        }

        var startLength = sqlBuilder.Length;
        var orderBy = " ORDER BY";
        sqlBuilder.Append(orderBy);
        foreach (var sort in _queryBody.Sorts)
        {
            if (sort.Mode == QuerySortMode.Lambda)
            {
                ResolveSort(sqlBuilder, sort.Lambda.Body, sort.Lambda, sort.Type);
            }
            else
            {
                sqlBuilder.AppendFormat(" {0} {1},", sort.Sql, sort.Type == SortType.Asc ? "ASC" : "DESC");
            }
        }

        if (startLength + orderBy.Length == sqlBuilder.Length)
        {
            sqlBuilder.Remove(sqlBuilder.Length - orderBy.Length, orderBy.Length);
        }
        else if (startLength + orderBy.Length < sqlBuilder.Length)
        {
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1);
        }
    }

    /// <summary>
    /// 解析排序
    /// </summary>
    private void ResolveSort(StringBuilder sqlBuilder, Expression exp, LambdaExpression fullExp, SortType sortType)
    {
        switch (exp.NodeType)
        {
            case ExpressionType.MemberAccess:
                //m => m.Title
                //m => m.Title.Length
                //m => m.T1.Title
                //m => m.T1.Length
                //m => m.T1.Title.Length
                ResolveSort(sqlBuilder, exp as MemberExpression, sortType);
                break;
            case ExpressionType.Convert:
                //m => m.Title.Length
                if (exp is UnaryExpression unaryExpression)
                {
                    ResolveSort(sqlBuilder, unaryExpression.Operand as MemberExpression, sortType);
                }
                break;
            case ExpressionType.Call:
                var callExp = exp as MethodCallExpression;
                var memberExp = callExp!.Object as MemberExpression;
                var columnName = _queryBody.GetColumnName(memberExp);
                object[] args = null;
                if (callExp.Arguments.Any())
                {
                    args = new object[callExp.Arguments.Count];
                    for (int i = 0; i < callExp.Arguments.Count; i++)
                    {
                        args[i] = ((ConstantExpression)callExp.Arguments[i]).Value;
                    }
                }
                sqlBuilder.AppendFormat(" {0} {1},", _dbAdapter.FunctionMapper(callExp.Method.Name, columnName, callExp.Object!.Type, args), sortType == SortType.Asc ? "ASC" : "DESC");
                break;
            case ExpressionType.New:
                //m=>new { m.Title.Substring(2) }
                if (exp is NewExpression newExp)
                {
                    foreach (var arg in newExp.Arguments)
                    {
                        ResolveSort(sqlBuilder, arg, fullExp, sortType);
                    }
                }
                break;
        }
    }

    /// <summary>
    /// 解析成员表达式中的排序信息
    /// </summary>
    private void ResolveSort(StringBuilder sqlBuilder, MemberExpression memberExp, SortType sortType)
    {
        var sort = sortType == SortType.Asc ? "ASC" : "DESC";
        switch (memberExp.Expression!.NodeType)
        {
            case ExpressionType.Parameter:
                //m => m.Title
                var columnName = _queryBody.GetColumnName(memberExp);
                sqlBuilder.AppendFormat(" {0} {1},", columnName, sort);
                break;
            case ExpressionType.MemberAccess:
                //m => m.Title.Length
                //m => m.T1.Title
                //m => m.T1.Length
                //m => m.T1.Title.Length
                if (memberExp.Expression.Type.IsString())
                {
                    columnName = _queryBody.GetColumnName(memberExp.Expression);
                    sqlBuilder.AppendFormat(" {0} {1},", _dbAdapter.FunctionMapper(memberExp.Member.Name, columnName), sort);
                }
                else if (DbConstants.ENTITY_INTERFACE_TYPE.IsImplementType(memberExp.Expression.Type))
                {
                    columnName = _queryBody.GetColumnName(memberExp);
                    sqlBuilder.AppendFormat(" {0} {1},", columnName, sort);
                }

                break;
        }
    }

    #endregion
}