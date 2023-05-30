using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Entities;
using Mkh.Data.Abstractions.Pagination;

namespace Mkh.Data.Core.Internal.QueryStructure;

/// <summary>
/// 查询主体信息
/// </summary>
public class QueryBody
{
    #region ==字段==

    private readonly IDbAdapter _dbAdapter;

    #endregion

    #region ==属性==

    /// <summary>
    /// 仓储
    /// </summary>
    public IRepository Repository { get; set; }

    public IDbAdapter DbAdapter => _dbAdapter;

    /// <summary>
    /// 是否使用参数化
    /// </summary>
    public bool UseParameters { get; set; } = true;

    /// <summary>
    /// 查询的列
    /// </summary>
    public QuerySelect Select { get; set; }

    /// <summary>
    /// 表连接信息
    /// </summary>
    public List<QueryJoin> Joins { get; } = new List<QueryJoin>();

    /// <summary>
    /// 过滤条件
    /// </summary>
    public List<QueryWhere> Wheres { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public List<QuerySort> Sorts { get; set; }

    /// <summary>
    /// 更新信息
    /// </summary>
    public QueryUpdate Update { get; set; }

    /// <summary>
    /// 是否分组查询
    /// </summary>
    public bool IsGroupBy { get; set; }

    /// <summary>
    /// 分组信息
    /// </summary>
    public LambdaExpression GroupBy { get; set; }

    /// <summary>
    /// 跳过行数
    /// </summary>
    public int Skip { get; set; }

    /// <summary>
    /// 取行数
    /// </summary>
    public int Take { get; set; }

    /// <summary>
    /// 过滤已删除的
    /// </summary>
    public bool FilterDeleted { get; set; } = true;

    /// <summary>
    /// 过滤租户
    /// </summary>
    public bool FilterTenant { get; set; } = true;

    /// <summary>
    /// 聚合过滤
    /// </summary>
    public List<QueryHaving> Havings { get; set; }

    /// <summary>
    /// 工作单元
    /// </summary>
    public IUnitOfWork Uow { get; set; }

    #endregion

    #region ==构造函数

    public QueryBody(IRepository repository)
    {
        Repository = repository;
        _dbAdapter = repository.DbContext.Adapter;
    }

    #endregion

    #region ==方法==

    #region ==设置排序==

    /// <summary>
    /// 设置排序
    /// </summary>
    /// <param name="field"></param>
    /// <param name="sortType"></param>
    public void SetSort(string field, SortType sortType)
    {
        if (field.IsNull())
            return;

        Sorts ??= new List<QuerySort>();

        Sorts.Add(new QuerySort { Mode = QuerySortMode.Sql, Sql = field, Type = sortType });
    }


    /// <summary>
    /// 设置排序
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="sortType"></param>
    public void SetSort(LambdaExpression expression, SortType sortType)
    {
        if (expression == null)
            return;

        Sorts ??= new List<QuerySort>();

        Sorts.Add(new QuerySort { Mode = QuerySortMode.Lambda, Lambda = expression, Type = sortType });
    }

    #endregion

    #region ==设置条件==

    public void SetWhere(LambdaExpression whereExpression)
    {
        if (whereExpression == null)
            return;

        Wheres ??= new List<QueryWhere>();

        Wheres.Add(new QueryWhere(whereExpression));
    }

    public void SetWhere(string whereSql)
    {
        if (whereSql.IsNull())
            return;

        Wheres ??= new List<QueryWhere>();

        Wheres.Add(new QueryWhere(whereSql));
    }

    public void SetWhere(LambdaExpression expression, string queryOperator, Abstractions.Queryable.IQueryable subQueryable)
    {
        if (subQueryable == null)
            return;

        Wheres ??= new List<QueryWhere>();

        Wheres.Add(new QueryWhere(expression, queryOperator, subQueryable));
    }

    #endregion

    #region ==设置选择列==

    /// <summary>
    /// 设置列
    /// </summary>
    /// <param name="selectExpression"></param>
    public void SetSelect(LambdaExpression selectExpression)
    {
        if (selectExpression == null)
            return;

        if (IsGroupBy && selectExpression.Body.NodeType != ExpressionType.New)
        {
            throw new ArgumentException("分组选择列必须使用匿名函数(m => new {})");
        }

        Select ??= new QuerySelect();

        Select.Mode = QuerySelectMode.Lambda;
        Select.Include = selectExpression;
    }

    /// <summary>
    /// 设置列
    /// </summary>
    /// <param name="sql"></param>
    public void SetSelect(string sql)
    {
        if (sql.IsNull())
            return;

        Select ??= new QuerySelect();

        Select.Mode = QuerySelectMode.Sql;
        Select.Sql = sql;
    }

    /// <summary>
    /// 设置函数选择列
    /// </summary>
    /// <param name="functionExpression">函数表达式</param>
    /// <param name="functionName">函数名称</param>
    public void SetFunctionSelect(LambdaExpression functionExpression, string functionName)
    {
        if (functionExpression == null || functionName == null)
            return;

        Select ??= new QuerySelect();

        Select.Mode = QuerySelectMode.Function;
        Select.FunctionExpression = functionExpression;
        Select.FunctionName = functionName;
    }

    /// <summary>
    /// 设置排除列
    /// </summary>
    /// <param name="excludeExpression"></param>
    public void SetSelectExclude(LambdaExpression excludeExpression)
    {
        Select ??= new QuerySelect();

        Select.Exclude = excludeExpression;
    }

    #endregion

    #region ==设置分页==

    public void SetLimit(int skip, int take)
    {
        Skip = skip < 0 ? 0 : skip;
        Take = take < 0 ? 0 : take;
    }

    #endregion

    #region ==设置更新==

    public void SetUpdate(LambdaExpression expression)
    {
        Update ??= new QueryUpdate();

        Update.Mode = QueryUpdateMode.Lambda;
        Update.Lambda = expression;
    }

    public void SetUpdate(string sql)
    {
        Update ??= new QueryUpdate();

        Update.Mode = QueryUpdateMode.Sql;
        Update.Sql = sql;
    }

    #endregion

    #region ==获取列名==

    /// <summary>
    /// 从成员表达式中获取列名
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public string GetColumnName(Expression expression)
    {
        var join = GetJoin(expression as MemberExpression);
        if (join == null) return null;

        var columnName = join.Item2.Name;
        //只有一个实体的时候，不需要别名
        if (Joins.Count == 1)
        {
            return _dbAdapter.AppendQuote(columnName);
        }

        return $"{join.Item1.Alias}.{_dbAdapter.AppendQuote(columnName)}";
    }

    /// <summary>
    /// 获取完整列名
    /// </summary>
    /// <param name="join"></param>
    /// <param name="descriptor"></param>
    /// <returns></returns>
    public string GetColumnName(QueryJoin join, IColumnDescriptor descriptor)
    {
        var columnName = descriptor.Name;
        //只有一个实体的时候，不需要别名
        if (Joins.Count == 1)
        {
            return _dbAdapter.AppendQuote(columnName);
        }

        return $"{join.Alias}.{_dbAdapter.AppendQuote(columnName)}";
    }

    #endregion

    #region ==获取列信息==

    /// <summary>
    /// 获取列描述信息
    /// </summary>
    /// <param name="name"></param>
    /// <param name="join"></param>
    /// <returns></returns>
    public IColumnDescriptor GetColumnDescriptor(string name, QueryJoin join)
    {
        var col = join.EntityDescriptor.Columns.FirstOrDefault(m => m.PropertyInfo.Name.Equals(name));

        Check.NotNull(col, nameof(col), $"({name})列不存在");

        return col;
    }

    #endregion

    #region ==获取表连接信息==

    public Tuple<QueryJoin, IColumnDescriptor> GetJoin(MemberExpression exp)
    {
        var fieldExp = exp.Expression;

        if (typeof(IEntity).IsImplementType(fieldExp!.Type))
        {
            var join = Joins.First(m => m.EntityDescriptor.EntityType == fieldExp.Type);
            var column = GetColumnDescriptor(exp.Member.Name, join);
            return new Tuple<QueryJoin, IColumnDescriptor>(join, column);
        }

        if (fieldExp.NodeType == ExpressionType.MemberAccess)
        {
            return GetJoin(fieldExp as MemberExpression);
        }

        return null;
    }

    #endregion

    #region ==设置分组条件==

    public void SetGroupBy(Expression groupByExpression)
    {
        if (groupByExpression == null)
            return;

        GroupBy = groupByExpression as LambdaExpression;
    }

    #endregion

    #region ==设置聚合过滤条件==

    /// <summary>
    /// 设置聚合过滤条件
    /// </summary>
    /// <param name="havingExpression"></param>
    public void SetHaving(LambdaExpression havingExpression)
    {
        if (havingExpression == null)
            return;

        Havings ??= new List<QueryHaving>();

        Havings.Add(new QueryHaving(havingExpression));
    }

    /// <summary>
    /// 设置聚合过滤条件
    /// </summary>
    /// <param name="havingSql"></param>
    public void SetHaving(string havingSql)
    {
        if (havingSql.IsNull())
            return;

        Havings ??= new List<QueryHaving>();

        Havings.Add(new QueryHaving(havingSql));
    }

    #endregion

    #region ==设置工作单元==

    /// <summary>
    /// 设置工作单元
    /// </summary>
    /// <param name="uow"></param>
    public void SetUow(IUnitOfWork uow)
    {
        Uow = uow;
    }

    #endregion

    #endregion

    #region ==Copy==

    public QueryBody Copy()
    {
        var copy = new QueryBody(Repository)
        {
            Repository = Repository,
            FilterDeleted = FilterDeleted,
            FilterTenant = FilterTenant,
            GroupBy = GroupBy,
            Skip = Skip,
            Take = Take
        };

        copy.Joins.AddRange(Joins);

        if (Wheres != null)
        {
            copy.Wheres = new List<QueryWhere>();
            copy.Wheres.AddRange(Wheres);
        }

        if (Sorts != null)
        {
            copy.Sorts = new List<QuerySort>();
            copy.Sorts.AddRange(Sorts);
        }

        if (Update != null)
        {
            copy.Update = new QueryUpdate { Lambda = Update.Lambda, Sql = Update.Sql, Mode = Update.Mode };
        }

        if (Select != null)
        {
            copy.Select = new QuerySelect
            {
                Sql = Select.Sql,
                FunctionName = Select.FunctionName,
                Exclude = Select.Exclude,
                FunctionExpression = Select.FunctionExpression,
                Include = Select.Include,
                Mode = Select.Mode
            };
        }

        if (Havings != null)
        {
            copy.Havings = new List<QueryHaving>();
            copy.Havings.AddRange(Havings);
        }

        return copy;
    }

    #endregion

    /// <summary>
    /// 获取实体描述符
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public IEntityDescriptor GetEntityDescriptor<TEntity>() where TEntity : IEntity
    {
        return Repository.DbContext.EntityDescriptors.First(m => m.EntityType == typeof(TEntity));
    }
}