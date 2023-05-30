using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Entities;
using Mkh.Data.Abstractions.Pagination;
using Mkh.Data.Abstractions.Query;
using Mkh.Data.Abstractions.Queryable;
using Mkh.Data.Core.Internal.QueryStructure;
using IQueryable = Mkh.Data.Abstractions.Queryable.IQueryable;

namespace Mkh.Data.Core.Queryable;

internal class Queryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> : Queryable, IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>
    where TEntity : IEntity, new()
    where TEntity2 : IEntity, new()
    where TEntity3 : IEntity, new()
    where TEntity4 : IEntity, new()
    where TEntity5 : IEntity, new()
    where TEntity6 : IEntity, new()
    where TEntity7 : IEntity, new()
    where TEntity8 : IEntity, new()
    where TEntity9 : IEntity, new()
    where TEntity10 : IEntity, new()
    where TEntity11 : IEntity, new()
    where TEntity12 : IEntity, new()
    where TEntity13 : IEntity, new()
    where TEntity14 : IEntity, new()
    where TEntity15 : IEntity, new()
{
    public Queryable(QueryBody queryBody, JoinType joinType, Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, bool>> onExpression, string tableName, bool noLock) : base(queryBody)
    {
        var entityDescriptor = _queryBody.GetEntityDescriptor<TEntity15>();
        var join = new QueryJoin(entityDescriptor, "T15", joinType, onExpression, noLock);
        join.TableName = tableName.NotNull() ? tableName : entityDescriptor.TableName;

        _queryBody.Joins.Add(join);
    }

    private Queryable(QueryBody queryBody) : base(queryBody)
    {

    }

    #region ==Sort==

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> OrderBy(string field)
    {
        _queryBody.SetSort(field, SortType.Asc);
        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> OrderByDescending(string field)
    {
        _queryBody.SetSort(field, SortType.Desc);
        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> OrderBy<TKey>(Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, TKey>> expression)
    {
        _queryBody.SetSort(expression, SortType.Asc);
        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> OrderByDescending<TKey>(Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, TKey>> expression)
    {
        _queryBody.SetSort(expression, SortType.Desc);
        return this;
    }

    #endregion

    #region ==Where==

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> Where(Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, bool>> expression)
    {
        _queryBody.SetWhere(expression);
        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> Where(string whereSql)
    {
        _queryBody.SetWhere(whereSql);
        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> WhereIf(bool condition, Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, bool>> expression)
    {
        if (condition)
        {
            _queryBody.SetWhere(expression);
        }
        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> WhereIf(bool condition, string whereSql)
    {
        if (condition)
        {
            _queryBody.SetWhere(whereSql);
        }

        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> WhereIfElse(bool condition, Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, bool>> ifExpression, Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, bool>> elseExpression)
    {
        _queryBody.SetWhere(condition ? ifExpression : elseExpression);
        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> WhereIfElse(bool condition, string ifWhereSql, string elseWhereSql)
    {
        _queryBody.SetWhere(condition ? ifWhereSql : elseWhereSql);
        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> WhereNotNull(string condition, Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, bool>> expression)
    {
        if (condition.NotNull())
        {
            _queryBody.SetWhere(expression);
        }

        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> WhereNotNull(string condition, Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, bool>> ifExpression, Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, bool>> elseExpression)
    {
        _queryBody.SetWhere(condition.NotNull() ? ifExpression : elseExpression);
        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> WhereNotNull(string condition, string whereSql)
    {
        if (condition.NotNull())
        {
            _queryBody.SetWhere(whereSql);
        }

        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> WhereNotNull(string condition, string ifWhereSql, string elseWhereSql)
    {
        _queryBody.SetWhere(condition.NotNull() ? ifWhereSql : elseWhereSql);
        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> WhereNotNull(object condition, Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, bool>> expression)
    {
        if (condition != null)
        {
            _queryBody.SetWhere(expression);
        }

        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> WhereNotNull(object condition, Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, bool>> ifExpression, Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, bool>> elseExpression)
    {
        _queryBody.SetWhere(condition != null ? ifExpression : elseExpression);
        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> WhereNotNull(object condition, string whereSql)
    {
        if (condition != null)
        {
            _queryBody.SetWhere(whereSql);
        }

        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> WhereNotNull(object condition, string ifWhereSql, string elseWhereSql)
    {
        _queryBody.SetWhere(condition != null ? ifWhereSql : elseWhereSql);
        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> WhereNotEmpty(Guid condition, Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, bool>> expression)
    {
        if (condition != Guid.Empty)
        {
            _queryBody.SetWhere(expression);
        }

        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> WhereNotEmpty(Guid condition, string whereSql)
    {
        if (condition != Guid.Empty)
        {
            _queryBody.SetWhere(whereSql);
        }

        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> WhereNotEmpty(Guid condition, Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, bool>> ifExpression, Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, bool>> elseExpression)
    {
        _queryBody.SetWhere(condition != Guid.Empty ? ifExpression : elseExpression);
        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> WhereNotEmpty(Guid condition, string ifWhereSql, string elseWhereSql)
    {
        _queryBody.SetWhere(condition != Guid.Empty ? ifWhereSql : elseWhereSql);
        return this;
    }

    #endregion

    #region ==SubQuery==

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> SubQueryEqual<TKey>(Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, TKey>> key, IQueryable queryable)
    {
        _queryBody.SetWhere(key, "=", queryable);
        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> SubQueryNotEqual<TKey>(Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, TKey>> key, IQueryable queryable)
    {
        _queryBody.SetWhere(key, "<>", queryable);
        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> SubQueryGreaterThan<TKey>(Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, TKey>> key, IQueryable queryable)
    {
        _queryBody.SetWhere(key, ">", queryable);
        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> SubQueryGreaterThanOrEqual<TKey>(Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, TKey>> key, IQueryable queryable)
    {
        _queryBody.SetWhere(key, ">=", queryable);
        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> SubQueryLessThan<TKey>(Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, TKey>> key, IQueryable queryable)
    {
        _queryBody.SetWhere(key, "<", queryable);
        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> SubQueryLessThanOrEqual<TKey>(Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, TKey>> key, IQueryable queryable)
    {
        _queryBody.SetWhere(key, "<=", queryable);
        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> SubQueryIn<TKey>(Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, TKey>> key, IQueryable queryable)
    {
        _queryBody.SetWhere(key, "IN", queryable);
        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> SubQueryNotIn<TKey>(Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, TKey>> key, IQueryable queryable)
    {
        _queryBody.SetWhere(key, "NOT IN", queryable);
        return this;
    }

    #endregion

    #region ==Limit==

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> Limit(int skip, int take)
    {
        _queryBody.SetLimit(skip, take);
        return this;
    }

    #endregion

    #region ==Select==

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> Select<TResult>(Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, TResult>> expression)
    {
        _queryBody.SetSelect(expression);
        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> Select<TResult>(string sql)
    {
        _queryBody.SetSelect(sql);
        return this;
    }

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> SelectExclude<TResult>(Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, TResult>> expression)
    {
        _queryBody.SetSelectExclude(expression);
        return this;
    }

    #endregion

    #region ==List==

    public Task<IList<TEntity>> ToList()
    {
        return ToList<TEntity>();
    }

    #endregion

    #region ==Pagination==

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

    #region ==First==

    public Task<TEntity> ToFirst()
    {
        return ToFirst<TEntity>();
    }

    #endregion

    #region ==NotFilterSoftDeleted==

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> NotFilterSoftDeleted()
    {
        _queryBody.FilterDeleted = false;
        return this;
    }

    #endregion

    #region ==NotFilterTenant==

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> NotFilterTenant()
    {
        _queryBody.FilterTenant = false;
        return this;
    }

    #endregion

    #region ==Function==

    public Task<TResult> ToMax<TResult>(Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, TResult>> expression)
    {
        return base.ToMax<TResult>(expression);
    }

    public Task<TResult> ToMin<TResult>(Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, TResult>> expression)
    {
        return base.ToMin<TResult>(expression);
    }

    public Task<TResult> ToSum<TResult>(Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, TResult>> expression)
    {
        return base.ToSum<TResult>(expression);
    }

    public Task<TResult> ToAvg<TResult>(Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>, TResult>> expression)
    {
        return base.ToAvg<TResult>(expression);
    }

    #endregion

    #region ==Copy==

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> Copy()
    {
        return new Queryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15>(_queryBody.Copy());
    }

    #endregion

    #region ==Uow==

    public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7, TEntity8, TEntity9, TEntity10, TEntity11, TEntity12, TEntity13, TEntity14, TEntity15> UseUow(IUnitOfWork uow)
    {
        _queryBody.SetUow(uow);
        return this;
    }

    #endregion
}