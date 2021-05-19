using System;
using System.Linq.Expressions;
#if DEBUG
using System.Runtime.CompilerServices;
#endif
using System.Text;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Pagination;
using Mkh.Data.Abstractions.Queryable;
using Mkh.Data.Core.Internal;
using Mkh.Data.Core.Internal.QueryStructure;

#if DEBUG
[assembly: InternalsVisibleTo("Data.Adapter.MySql.Test")]
[assembly: InternalsVisibleTo("Data.Adapter.SqlServer.Test")]
#endif
namespace Mkh.Data.Core.SqlBuilder
{
    /// <summary>
    /// 分组SQL生成器
    /// </summary>
    internal class GroupBySqlBuilder
    {
        private readonly QueryBody _queryBody;
        private readonly IDbAdapter _dbAdapter;

        public GroupBySqlBuilder(QueryBody queryBody)
        {
            _queryBody = queryBody;
            _dbAdapter = queryBody.Repository.DbContext.Adapter;
        }

        #region ==BuildListSql==

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
            ResolveGroupBy(sqlBuilder);
            ResolveHaving(sqlBuilder, parameters);
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
            var sql = BuildListSql(null);
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
            var group = ResolveGroupBy();
            var having = ResolveHaving(parameters);

            return _dbAdapter.GeneratePagingSql(select, from, where, sort, 0, 1, group, having);
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

        #region ==解析查询列==

        public string ResolveSelect()
        {
            var sqlBuilder = new StringBuilder();
            ResolveSelect(sqlBuilder);
            return sqlBuilder.ToString();
        }

        public void ResolveSelect(StringBuilder sqlBuilder)
        {
            var select = _queryBody.Select;
            if (select == null)
                throw new ArgumentException("未指定查询信息");

            if (select.Mode == QuerySelectMode.Sql)
            {
                //SQL语句
                sqlBuilder.Append(select.Sql);
            }
            else if (select.Mode == QuerySelectMode.Lambda)
            {
                //表达式
                var exp = select.Include.Body;
                if (exp.NodeType != ExpressionType.New)
                {
                    throw new ArgumentException("分组查询的列，必须使用匿名对象的方式");
                }

                var newExp = exp as NewExpression;
                for (var i = 0; i < newExp.Arguments.Count; i++)
                {
                    var arg = newExp.Arguments[i];
                    var alias = newExp.Members![i].Name;
                    string columnName;

                    switch (arg.NodeType)
                    {
                        case ExpressionType.MemberAccess:
                            var memExp = arg as MemberExpression;
                            columnName = GetColumnName(memExp);
                            sqlBuilder.AppendFormat("{0} AS {1},", columnName, _dbAdapter.AppendQuote(alias));
                            break;
                        case ExpressionType.Call:
                            if (arg is MethodCallExpression callExp)
                            {
                                var methodName = callExp.Method.Name;
                                switch (callExp.Object!.NodeType)
                                {
                                    case ExpressionType.Parameter:
                                        if (callExp.Arguments.Count > 0)
                                        {
                                            //m => m.Sum(x => x.Id)
                                            var s = (callExp.Arguments[0] as UnaryExpression)!.Operand as LambdaExpression;
                                            var memberExp = s.Body as MemberExpression;
                                            columnName = _queryBody.GetColumnName(memberExp);
                                            sqlBuilder.AppendFormat("{0} AS {1},", _dbAdapter.FunctionMapper(callExp.Method.Name, columnName, callExp.Object!.Type), _dbAdapter.AppendQuote(alias));
                                        }
                                        else
                                        {
                                            sqlBuilder.AppendFormat("{0} AS {1},", _dbAdapter.FunctionMapper(methodName, null), _dbAdapter.AppendQuote(alias));
                                        }
                                        break;
                                    case ExpressionType.MemberAccess:
                                        //m => m.Key.Title.Substring(3)
                                        columnName = GetColumnName(callExp!.Object as MemberExpression);
                                        var args = ExpressionResolver.Arguments2Object(callExp.Arguments);
                                        sqlBuilder.AppendFormat("{0} AS {1},", _dbAdapter.FunctionMapper(methodName, columnName, callExp.Object.Type, args), _dbAdapter.AppendQuote(alias));
                                        break;
                                    default:
                                        sqlBuilder.AppendFormat("{0} AS {1},", _dbAdapter.FunctionMapper(methodName, null), _dbAdapter.AppendQuote(alias));
                                        break;
                                }
                            }
                            break;
                    }
                }
            }

            //移除末尾的逗号
            if (sqlBuilder.Length > 0 && sqlBuilder[^1] == ',')
            {
                sqlBuilder.Remove(sqlBuilder.Length - 1, 1);
            }
        }

        #endregion

        #region ==解析分组条件==

        private string ResolveGroupBy()
        {
            var sqlBuilder = new StringBuilder();
            ResolveGroupBy(sqlBuilder);
            return sqlBuilder.ToString();
        }

        private void ResolveGroupBy(StringBuilder sqlBuilder)
        {
            if (_queryBody.GroupBy.Body.NodeType != ExpressionType.New)
            {
                throw new ArgumentException("分组表达式必须使用匿名函数方式");
            }

            var newExp = _queryBody.GroupBy.Body as NewExpression;

            sqlBuilder.Append(" GROUP BY");

            for (var i = 0; i < newExp!.Arguments.Count; i++)
            {
                var arg = newExp.Arguments[i];
                switch (arg.NodeType)
                {
                    case ExpressionType.MemberAccess:
                        var memExp = arg as MemberExpression;
                        if (memExp!.Expression.Type.IsString())
                        {
                            //字符串的Length属性
                            var columnName = _queryBody.GetColumnName(memExp.Expression);
                            sqlBuilder.AppendFormat(" {0},", _queryBody.DbAdapter.FunctionMapper(memExp.Member.Name, columnName));
                        }
                        else if (DbConstants.ENTITY_INTERFACE_TYPE.IsImplementType(memExp.Expression.Type))
                        {
                            //解析列信息，如：m.T1.Id
                            var columnName = _queryBody.GetColumnName(memExp);
                            sqlBuilder.AppendFormat(" {0},", columnName);
                        }
                        break;
                    case ExpressionType.Call:
                        if (arg is MethodCallExpression callExp)
                        {
                            var columnName = _queryBody.GetColumnName(callExp!.Object);
                            var args = ExpressionResolver.Arguments2Object(callExp.Arguments);
                            sqlBuilder.AppendFormat(" {0},", _dbAdapter.FunctionMapper(callExp.Method.Name, columnName, callExp.Object!.Type, args));
                        }
                        break;
                }
            }

            //移除末尾的逗号
            if (sqlBuilder[^1] == ',')
            {
                sqlBuilder.Remove(sqlBuilder.Length - 1, 1);
            }
        }

        #endregion

        #region ==解析聚合条件==

        private string ResolveHaving(IQueryParameters parameters)
        {
            var sqlBuilder = new StringBuilder();
            ResolveHaving(sqlBuilder, parameters);
            return sqlBuilder.ToString();
        }

        private void ResolveHaving(StringBuilder sqlBuilder, IQueryParameters parameters)
        {
            if (_queryBody.Havings.NotNullAndEmpty())
            {
                sqlBuilder.Append(" HAVING");
                foreach (var having in _queryBody.Havings)
                {
                    if (having.Mode == QueryHavingMode.Sql)
                    {
                        sqlBuilder.Append(having.Sql);
                    }
                    else
                    {
                        ExpressionResolver.Resolve(_queryBody, having.Lambda.Body, having.Lambda, sqlBuilder, parameters);
                    }
                }
            }
        }

        #endregion

        #region ==解析排序==

        public string ResolveSort()
        {
            var sqlBuilder = new StringBuilder();
            ResolveSort(sqlBuilder);
            return sqlBuilder.ToString();
        }

        public void ResolveSort(StringBuilder sqlBuilder)
        {
            if (_queryBody.Sorts.IsNullOrEmpty())
                return;

            var startLength = sqlBuilder.Length;
            sqlBuilder.Append(" ORDER BY");

            foreach (var sort in _queryBody.Sorts)
            {
                if (sort.Mode == QuerySortMode.Lambda)
                {
                    ResolveSort(sqlBuilder, sort.Lambda.Body, sort.Type);
                }
                else
                {
                    sqlBuilder.AppendFormat(" {0} {1},", sort.Sql, sort.Type == SortType.Asc ? "ASC" : "DESC");
                }
            }

            if (startLength + 9 == sqlBuilder.Length)
            {
                sqlBuilder.Remove(sqlBuilder.Length - 9, 9);
            }
            else if (startLength + 9 < sqlBuilder.Length)
            {
                sqlBuilder.Remove(sqlBuilder.Length - 1, 1);
            }
        }

        /// <summary>
        /// 解析排序
        /// </summary>
        private void ResolveSort(StringBuilder sqlBuilder, Expression exp, SortType sortType)
        {
            var sort = sortType == SortType.Asc ? "ASC" : "DESC";
            switch (exp.NodeType)
            {
                case ExpressionType.MemberAccess:
                    ResolveSort(sqlBuilder, exp as MemberExpression, sort);
                    break;
                case ExpressionType.Call:
                    var callExp = exp as MethodCallExpression;
                    var methodName = callExp.Method.Name;
                    switch (callExp.Object.NodeType)
                    {
                        case ExpressionType.Parameter:
                            //OrderBy(m => m.Sum(x => x.Id))
                            var fullLambda = (callExp.Arguments[0] as UnaryExpression).Operand as LambdaExpression;
                            var columnName = _queryBody.GetColumnName(fullLambda.Body);
                            sqlBuilder.AppendFormat(" {0} {1},", _queryBody.DbAdapter.FunctionMapper(methodName, columnName), sort);
                            break;
                        case ExpressionType.MemberAccess:
                            //OrderBy(m => m.Key.Title.Substring(3))
                            columnName = GetColumnName(callExp!.Object as MemberExpression);
                            var args = ExpressionResolver.Arguments2Object(callExp.Arguments);
                            sqlBuilder.AppendFormat(" {0} {1},", _dbAdapter.FunctionMapper(methodName, columnName, callExp.Object!.Type, args), sort);
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// 解析成员表达式中的排序信息
        /// </summary>
        private void ResolveSort(StringBuilder sqlBuilder, MemberExpression memberExp, string sort)
        {
            switch (memberExp.Expression!.NodeType)
            {
                case ExpressionType.MemberAccess:
                    sqlBuilder.AppendFormat(" {0} {1},", GetColumnName(memberExp), sort);
                    break;
            }
        }

        #endregion

        /// <summary>
        /// 获取列名
        /// </summary>
        /// <param name="memberExp"></param>
        private string GetColumnName(MemberExpression memberExp)
        {
            /*
             * 通过比对分组表达式中成员名称与选择列的Key中的成员名称来判断使用的是哪一个实体
             * 分组表达式：m => new { m.Title }
             * 选择列表达式：
             * m => new
                {
                    Sum = m.Sum(n => n.Id),
                    m.Key.Title
                }
             */

            //获取查询列对应的实体
            var groupByExp = _queryBody.GroupBy.Body as NewExpression;
            for (var t = 0; t < groupByExp.Members.Count; t++)
            {
                if (groupByExp.Members[t].Name.Equals(memberExp.Member.Name))
                {
                    var groupByArg = groupByExp.Arguments[t];
                    string columnName;
                    switch (groupByArg.NodeType)
                    {
                        case ExpressionType.MemberAccess:
                            var memExp = groupByArg as MemberExpression;
                            columnName = _queryBody.GetColumnName(memExp);
                            if (memExp!.Expression!.Type.IsString())
                            {
                                return _queryBody.DbAdapter.FunctionMapper(memExp.Member.Name, columnName);
                            }
                            return columnName;
                        case ExpressionType.Call:
                            var callExp = groupByArg as MethodCallExpression;
                            columnName = _queryBody.GetColumnName(callExp!.Object);
                            var args = ExpressionResolver.Arguments2Object(callExp.Arguments);
                            return _dbAdapter.FunctionMapper(callExp.Method.Name, columnName, callExp.Object!.Type, args);
                        default:
                            throw new Exception("不支持的语法");
                    }
                }
            }
            throw new Exception("不支持的语法");
        }
    }
}
