using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Entities;
using Mkh.Data.Abstractions.Pagination;
using Mkh.Data.Abstractions.Query;
using Mkh.Data.Abstractions.Queryable;
using Mkh.Data.Abstractions.Queryable.Grouping;
using Mkh.Data.Core.Internal.QueryStructure;
using Mkh.Data.Core.Queryable.Grouping;

namespace Mkh.Data.Core.Queryable;

internal class Queryable<TEntity> : Queryable, IQueryable<TEntity> where TEntity : IEntity, new()
{
    public Queryable(IRepository repository, Expression<Func<TEntity, bool>> expression, string tableName, bool noLock) : base(repository)
    {
        var entityDescriptor = _queryBody.GetEntityDescriptor<TEntity>();
        var t1 = new QueryJoin(repository.EntityDescriptor, "T1", JoinType.UnKnown, null, noLock);
        t1.TableName = tableName.NotNull() ? tableName : entityDescriptor.TableName;
        _queryBody.Joins.Add(t1);
        _queryBody.SetWhere(expression);
    }

    private Queryable(QueryBody queryBody) : base(queryBody)
    {

    }

    #region ==排序==

    public IQueryable<TEntity> OrderBy(string field)
    {
        _queryBody.SetSort(field, SortType.Asc);
        return this;
    }

    public IQueryable<TEntity> OrderByDescending(string field)
    {
        _queryBody.SetSort(field, SortType.Desc);
        return this;
    }

    public IQueryable<TEntity> OrderBy<TKey>(Expression<Func<TEntity, TKey>> expression)
    {
        _queryBody.SetSort(expression, SortType.Asc);
        return this;
    }

    public IQueryable<TEntity> OrderByDescending<TKey>(Expression<Func<TEntity, TKey>> expression)
    {
        _queryBody.SetSort(expression, SortType.Desc);
        return this;
    }

    #endregion

    #region ==过滤条件=

    public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
    {
        _queryBody.SetWhere(expression);
        return this;
    }

    public IQueryable<TEntity> Where(string whereSql)
    {
        _queryBody.SetWhere(whereSql);
        return this;
    }

    public IQueryable<TEntity> WhereIf(bool condition, Expression<Func<TEntity, bool>> expression)
    {
        if (condition)
        {
            _queryBody.SetWhere(expression);
        }
        return this;
    }

    public IQueryable<TEntity> WhereIf(bool condition, string whereSql)
    {
        if (condition)
        {
            _queryBody.SetWhere(whereSql);
        }

        return this;
    }

    public IQueryable<TEntity> WhereIfElse(bool condition, Expression<Func<TEntity, bool>> ifExpression, Expression<Func<TEntity, bool>> elseExpression)
    {
        _queryBody.SetWhere(condition ? ifExpression : elseExpression);
        return this;
    }

    public IQueryable<TEntity> WhereIfElse(bool condition, string ifWhereSql, string elseWhereSql)
    {
        _queryBody.SetWhere(condition ? ifWhereSql : elseWhereSql);
        return this;
    }

    public IQueryable<TEntity> WhereNotNull(string condition, Expression<Func<TEntity, bool>> expression)
    {
        if (condition.NotNull())
        {
            _queryBody.SetWhere(expression);
        }

        return this;
    }

    public IQueryable<TEntity> WhereNotNull(string condition, Expression<Func<TEntity, bool>> ifExpression, Expression<Func<TEntity, bool>> elseExpression)
    {
        _queryBody.SetWhere(condition.NotNull() ? ifExpression : elseExpression);
        return this;
    }

    public IQueryable<TEntity> WhereNotNull(string condition, string whereSql)
    {
        if (condition.NotNull())
        {
            _queryBody.SetWhere(whereSql);
        }

        return this;
    }

    public IQueryable<TEntity> WhereNotNull(string condition, string ifWhereSql, string elseWhereSql)
    {
        _queryBody.SetWhere(condition.NotNull() ? ifWhereSql : elseWhereSql);
        return this;
    }

    public IQueryable<TEntity> WhereNotNull(object condition, Expression<Func<TEntity, bool>> expression)
    {
        if (condition != null)
        {
            _queryBody.SetWhere(expression);
        }

        return this;
    }

    public IQueryable<TEntity> WhereNotNull(object condition, Expression<Func<TEntity, bool>> ifExpression, Expression<Func<TEntity, bool>> elseExpression)
    {
        _queryBody.SetWhere(condition != null ? ifExpression : elseExpression);
        return this;
    }

    public IQueryable<TEntity> WhereNotNull(object condition, string whereSql)
    {
        if (condition != null)
        {
            _queryBody.SetWhere(whereSql);
        }

        return this;
    }

    public IQueryable<TEntity> WhereNotNull(object condition, string ifWhereSql, string elseWhereSql)
    {
        _queryBody.SetWhere(condition != null ? ifWhereSql : elseWhereSql);
        return this;
    }

    public IQueryable<TEntity> WhereNotEmpty(Guid condition, Expression<Func<TEntity, bool>> expression)
    {
        if (condition != Guid.Empty)
        {
            _queryBody.SetWhere(expression);
        }

        return this;
    }

    public IQueryable<TEntity> WhereNotEmpty(Guid condition, string whereSql)
    {
        if (condition != Guid.Empty)
        {
            _queryBody.SetWhere(whereSql);
        }

        return this;
    }

    public IQueryable<TEntity> WhereNotEmpty(Guid condition, Expression<Func<TEntity, bool>> ifExpression, Expression<Func<TEntity, bool>> elseExpression)
    {
        _queryBody.SetWhere(condition != Guid.Empty ? ifExpression : elseExpression);
        return this;
    }

    public IQueryable<TEntity> WhereNotEmpty(Guid condition, string ifWhereSql, string elseWhereSql)
    {
        _queryBody.SetWhere(condition != Guid.Empty ? ifWhereSql : elseWhereSql);
        return this;
    }

    #endregion

    #region ==子查询==

    public IQueryable<TEntity> SubQueryEqual<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable)
    {
        _queryBody.SetWhere(key, "=", queryable);
        return this;
    }

    public IQueryable<TEntity> SubQueryNotEqual<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable)
    {
        _queryBody.SetWhere(key, "<>", queryable);
        return this;
    }

    public IQueryable<TEntity> SubQueryGreaterThan<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable)
    {
        _queryBody.SetWhere(key, ">", queryable);
        return this;
    }

    public IQueryable<TEntity> SubQueryGreaterThanOrEqual<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable)
    {
        _queryBody.SetWhere(key, ">=", queryable);
        return this;
    }

    public IQueryable<TEntity> SubQueryLessThan<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable)
    {
        _queryBody.SetWhere(key, "<", queryable);
        return this;
    }

    public IQueryable<TEntity> SubQueryLessThanOrEqual<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable)
    {
        _queryBody.SetWhere(key, "<=", queryable);
        return this;
    }

    public IQueryable<TEntity> SubQueryIn<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable)
    {
        _queryBody.SetWhere(key, "IN", queryable);
        return this;
    }

    public IQueryable<TEntity> SubQueryNotIn<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable)
    {
        _queryBody.SetWhere(key, "NOT IN", queryable);
        return this;
    }

    #endregion

    #region ==查询列==

    public IQueryable<TEntity> Select<TResult>(Expression<Func<TEntity, TResult>> expression)
    {
        _queryBody.SetSelect(expression);
        return this;
    }

    public IQueryable<TEntity> Select<TResult>(string sql)
    {
        _queryBody.SetSelect(sql);
        return this;
    }

    public IQueryable<TEntity> SelectExclude<TResult>(Expression<Func<TEntity, TResult>> expression)
    {
        _queryBody.SetSelectExclude(expression);
        return this;
    }

    #endregion

    #region ==限制==

    public IQueryable<TEntity> Limit(int skip, int take)
    {
        _queryBody.SetLimit(skip, take);
        return this;
    }

    #endregion

    #region ==表连接==

    public IQueryable<TEntity, TEntity2> LeftJoin<TEntity2>(Expression<Func<IQueryableJoins<TEntity, TEntity2>, bool>> onExpression, string tableName = null, bool noLock = true) where TEntity2 : IEntity, new()
    {
        return new Queryable<TEntity, TEntity2>(_queryBody, JoinType.Left, onExpression, tableName, noLock);
    }

    public IQueryable<TEntity, TEntity2> InnerJoin<TEntity2>(Expression<Func<IQueryableJoins<TEntity, TEntity2>, bool>> onExpression, string tableName = null, bool noLock = true) where TEntity2 : IEntity, new()
    {
        return new Queryable<TEntity, TEntity2>(_queryBody, JoinType.Inner, onExpression, tableName, noLock);
    }

    public IQueryable<TEntity, TEntity2> RightJoin<TEntity2>(Expression<Func<IQueryableJoins<TEntity, TEntity2>, bool>> onExpression, string tableName = null, bool noLock = true) where TEntity2 : IEntity, new()
    {
        return new Queryable<TEntity, TEntity2>(_queryBody, JoinType.Right, onExpression, tableName, noLock);
    }

    #endregion

    #region ==不过滤软删除数据==

    public IQueryable<TEntity> NotFilterSoftDeleted()
    {
        _queryBody.FilterDeleted = false;
        return this;
    }

    #endregion

    #region ==不过滤租户==

    public IQueryable<TEntity> NotFilterTenant()
    {
        _queryBody.FilterTenant = false;
        return this;
    }

    #endregion

    #region ==更新==

    public async Task<bool> ToUpdate(Expression<Func<TEntity, TEntity>> expression)
    {
        return await ToUpdateWithAffectedRowsNumber(expression) > 0;
    }

    public Task<int> ToUpdateWithAffectedRowsNumber(Expression<Func<TEntity, TEntity>> expression)
    {
        _queryBody.SetUpdate(expression);

        var sql = _sqlBuilder.BuildUpdateSql(out IQueryParameters parameters);
        _logger.Write("Update", sql);
        return _repository.Execute(sql, parameters.ToDynamicParameters(), _queryBody.Uow);
    }

    public async Task<bool> ToUpdate(string updateSql, Dictionary<string, object> parameters = null)
    {
        return await ToUpdateWithAffectedRowsNumber(updateSql, parameters) > 0;
    }

    public Task<int> ToUpdateWithAffectedRowsNumber(string updateSql, Dictionary<string, object> parameters = null)
    {
        _queryBody.SetUpdate(updateSql);
        var sql = _sqlBuilder.BuildUpdateSql(out IQueryParameters p_);
        _logger.Write("Update", sql);
        var dynamicParameters = p_.ToDynamicParameters();
        if (parameters != null)
        {
            foreach (var p in parameters)
            {
                dynamicParameters.Add(p.Key, p.Value);
            }
        }

        return _repository.Execute(sql, dynamicParameters, _queryBody.Uow);
    }

    public string ToUpdateSql(Expression<Func<TEntity, TEntity>> expression)
    {
        _queryBody.SetUpdate(expression);
        return _sqlBuilder.BuildUpdateSql(out _);
    }

    public string ToUpdateSql(Expression<Func<TEntity, TEntity>> expression, out IQueryParameters parameters)
    {
        _queryBody.SetUpdate(expression);
        return _sqlBuilder.BuildUpdateSql(out parameters);
    }

    public string ToUpdateSql(Expression<Func<TEntity, TEntity>> expression, IQueryParameters parameters)
    {
        _queryBody.SetUpdate(expression);
        return _sqlBuilder.BuildUpdateSql(parameters);
    }

    public string ToUpdateSqlNotUseParameters(Expression<Func<TEntity, TEntity>> expression)
    {
        _queryBody.SetUpdate(expression);
        return _sqlBuilder.BuildUpdateSqlNotUseParameters();
    }

    public string ToUpdateSql(string updateSql)
    {
        _queryBody.SetUpdate(updateSql);
        return _sqlBuilder.BuildUpdateSql(out _);
    }

    public string ToUpdateSql(string updateSql, out IQueryParameters parameters)
    {
        _queryBody.SetUpdate(updateSql);
        return _sqlBuilder.BuildUpdateSql(out parameters);
    }

    public string ToUpdateSql(string updateSql, IQueryParameters parameters)
    {
        _queryBody.SetUpdate(updateSql);
        return _sqlBuilder.BuildUpdateSql(parameters);
    }

    public string ToUpdateSqlNotUseParameters(string updateSql)
    {
        _queryBody.SetUpdate(updateSql);
        return _sqlBuilder.BuildUpdateSqlNotUseParameters();
    }

    #endregion

    #region ==删除==

    public async Task<bool> ToDelete()
    {
        return await ToDeleteWithAffectedRowsNumber() > 0;
    }

    public Task<int> ToDeleteWithAffectedRowsNumber()
    {
        var sql = _sqlBuilder.BuildDeleteSql(out IQueryParameters parameters);
        _logger.Write("Delete", sql);
        return _repository.Execute(sql, parameters.ToDynamicParameters(), _queryBody.Uow);
    }

    public string ToDeleteSql()
    {
        return _sqlBuilder.BuildDeleteSql(out _);
    }

    public string ToDeleteSql(out IQueryParameters parameters)
    {
        return _sqlBuilder.BuildDeleteSql(out parameters);
    }

    public string ToDeleteSql(IQueryParameters parameters)
    {
        return _sqlBuilder.BuildDeleteSql(parameters);
    }

    public string ToDeleteSqlNotUseParameters()
    {
        return _sqlBuilder.BuildDeleteSqlNotUseParameters();
    }

    #endregion

    #region ==软删除==

    public async Task<bool> ToSoftDelete()
    {
        return await ToSoftDeleteWithAffectedRowsNumber() > 0;
    }

    public Task<int> ToSoftDeleteWithAffectedRowsNumber()
    {
        var sql = _sqlBuilder.BuildSoftDeleteSql(out IQueryParameters parameters);
        _logger.Write("Delete", sql);
        return _repository.Execute(sql, parameters.ToDynamicParameters(), _queryBody.Uow);
    }

    public string ToSoftDeleteSql()
    {
        return _sqlBuilder.BuildSoftDeleteSql(out _);
    }

    public string ToSoftDeleteSql(out IQueryParameters parameters)
    {
        return _sqlBuilder.BuildSoftDeleteSql(out parameters);
    }

    public string ToSoftDeleteSql(IQueryParameters parameters)
    {
        return _sqlBuilder.BuildSoftDeleteSql(parameters);
    }

    public string ToSoftDeleteSqlNotUseParameters()
    {
        return _sqlBuilder.BuildSoftDeleteSqlNotUseParameters();
    }

    #endregion

    #region ==列表==

    public Task<IList<TEntity>> ToList()
    {
        return ToList<TEntity>();
    }

    #endregion

    #region ==分页==

    public Task<IList<TEntity>> ToPagination()
    {
        return ToPagination<TEntity>();
    }

    public Task<IList<TEntity>> ToPagination(Paging paging)
    {
        return ToPagination<TEntity>(paging);
    }

    public Task<PagingQueryResultModel<TEntity>> ToPaginationResult(Paging paging)
    {
        return ToPaginationResult<TEntity>(paging);
    }

    #endregion

    #region ==查询第一条==

    public Task<TEntity> ToFirst()
    {
        return ToFirst<TEntity>();
    }

    #endregion

    #region ==函数查询==

    public Task<TResult> ToMax<TResult>(Expression<Func<TEntity, TResult>> expression)
    {
        return base.ToMax<TResult>(expression);
    }

    public Task<TResult> ToMin<TResult>(Expression<Func<TEntity, TResult>> expression)
    {
        return base.ToMin<TResult>(expression);
    }

    public Task<TResult> ToSum<TResult>(Expression<Func<TEntity, TResult>> expression)
    {
        return base.ToSum<TResult>(expression);
    }

    public Task<TResult> ToAvg<TResult>(Expression<Func<TEntity, TResult>> expression)
    {
        return base.ToAvg<TResult>(expression);
    }

    #endregion

    #region ==分组查询==

    public IGroupingQueryable<TResult, TEntity> GroupBy<TResult>(Expression<Func<TEntity, TResult>> expression)
    {
        return new GroupingQueryable<TResult, TEntity>(_sqlBuilder, _logger, expression);
    }

    #endregion

    #region ==复制==

    public IQueryable<TEntity> Copy()
    {
        return new Queryable<TEntity>(_queryBody.Copy());
    }

    #endregion

    #region ==使用工作单元==

    public IQueryable<TEntity> UseUow(IUnitOfWork uow)
    {
        _queryBody.SetUow(uow);
        return this;
    }

    #endregion
}