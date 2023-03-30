using System;
using System.Linq.Expressions;
using Mkh.Data.Abstractions.Queryable;
using Mkh.Data.Core.Queryable;

namespace Mkh.Data.Core.Repository;

public abstract partial class RepositoryAbstract<TEntity>
{
    public IQueryable<TEntity> Find()
    {
        return new Queryable<TEntity>(this, null, null, true);
    }

    public IQueryable<TEntity> Find(string tableName)
    {
        return new Queryable<TEntity>(this, null, tableName, true);
    }

    public IQueryable<TEntity> Find(string tableName, bool noLock)
    {
        return new Queryable<TEntity>(this, null, tableName, noLock);
    }

    public IQueryable<TEntity> Find(bool noLock)
    {
        return new Queryable<TEntity>(this, null, null, noLock);
    }

    public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
    {
        return new Queryable<TEntity>(this, expression, null, true);
    }

    public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression, string tableName)
    {
        return new Queryable<TEntity>(this, expression, tableName, true);
    }

    public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression, bool noLock)
    {
        return new Queryable<TEntity>(this, expression, null, noLock);
    }

    public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression, string tableName, bool noLock)
    {
        return new Queryable<TEntity>(this, expression, tableName, noLock);
    }
}