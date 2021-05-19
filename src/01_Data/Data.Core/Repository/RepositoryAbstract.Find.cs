using System;
using System.Linq.Expressions;
using Mkh.Data.Abstractions.Queryable;
using Mkh.Data.Core.Queryable;

namespace Mkh.Data.Core.Repository
{
    public abstract partial class RepositoryAbstract<TEntity>
    {
        public IQueryable<TEntity> Find()
        {
            return new Queryable<TEntity>(this, null, true);
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        {
            return new Queryable<TEntity>(this, expression, true);
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression, bool noLock)
        {
            return new Queryable<TEntity>(this, expression, noLock);
        }
    }
}
