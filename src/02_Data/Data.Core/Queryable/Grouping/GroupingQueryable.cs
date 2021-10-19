using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Logger;
using Mkh.Data.Abstractions.Queryable;
using Mkh.Data.Abstractions.Queryable.Grouping;
using Mkh.Data.Core.Internal.QueryStructure;
using Mkh.Data.Core.SqlBuilder;

namespace Mkh.Data.Core.Queryable.Grouping;

internal abstract class GroupingQueryable : IGroupingQueryable
{
    protected readonly QueryBody _queryBody;
    protected readonly GroupBySqlBuilder _sqlBuilder;
    protected readonly DbLogger _logger;
    protected readonly IRepository _repository;

    protected GroupingQueryable(QueryableSqlBuilder sqlBuilder, DbLogger logger, Expression expression)
    {
        _queryBody = sqlBuilder.QueryBody.Copy();
        _queryBody.IsGroupBy = true;
        _queryBody.SetGroupBy(expression);
        _sqlBuilder = new GroupBySqlBuilder(_queryBody);
        _logger = logger;
        _repository = _queryBody.Repository;
    }

    #region ==List==

    public Task<IList<dynamic>> ToListDynamic()
    {
        return ToList<dynamic>();
    }

    public async Task<IList<TResult>> ToList<TResult>()
    {
        var sql = _sqlBuilder.BuildListSql(out IQueryParameters parameters);
        _logger.Write("GroupByToList", sql);
        return (await _repository.Query<TResult>(sql, parameters.ToDynamicParameters(), _queryBody.Uow)).ToList();
    }

    public string ToListSql()
    {
        return _sqlBuilder.BuildListSql(out _);
    }

    public string ToListSql(out IQueryParameters parameters)
    {
        return _sqlBuilder.BuildListSql(out parameters);
    }

    public string ToListSql(IQueryParameters parameters)
    {
        return _sqlBuilder.BuildListSql(parameters);
    }

    public string ToListSqlNotUseParameters()
    {
        return _sqlBuilder.BuildListSqlNotUseParameters();
    }

    #endregion

    #region ==Reader==

    public Task<IDataReader> ToReader()
    {
        var sql = _sqlBuilder.BuildListSql(out IQueryParameters parameters);
        _logger.Write("GroupByToReader", sql);
        return _repository.ExecuteReader(sql, parameters.ToDynamicParameters(), _queryBody.Uow);
    }

    #endregion

    #region ==First==

    public Task<dynamic> ToFirstDynamic()
    {
        return ToFirst<dynamic>();
    }

    public Task<TResult> ToFirst<TResult>()
    {
        var sql = _sqlBuilder.BuildFirstSql(out IQueryParameters parameters);
        _logger.Write("ToFirst", sql);
        return _repository.QueryFirstOrDefault<TResult>(sql, parameters.ToDynamicParameters(), _queryBody.Uow);
    }

    public string ToFirstSql()
    {
        return _sqlBuilder.BuildFirstSql(out _);
    }

    public string ToFirstSql(out IQueryParameters parameters)
    {
        return _sqlBuilder.BuildFirstSql(out parameters);
    }

    public string ToFirstSql(IQueryParameters parameters)
    {
        return _sqlBuilder.BuildFirstSql(parameters);
    }

    public string ToFirstSqlNotUseParameters()
    {
        return _sqlBuilder.BuildFirstSqlNotUserParameters();
    }

    #endregion
}