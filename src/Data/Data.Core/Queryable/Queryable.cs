using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Logger;
using Mkh.Data.Abstractions.Pagination;
using Mkh.Data.Abstractions.Query;
using Mkh.Data.Abstractions.Queryable;
using Mkh.Data.Core.Internal.QueryStructure;
using Mkh.Data.Core.SqlBuilder;
using IQueryable = Mkh.Data.Abstractions.Queryable.IQueryable;

namespace Mkh.Data.Core.Queryable;

internal class Queryable : IQueryable
{
    protected readonly IRepository _repository;
    protected readonly QueryBody _queryBody;
    protected readonly QueryableSqlBuilder _sqlBuilder;
    protected readonly DbLogger _logger;

    public Queryable(IRepository repository)
    {
        _repository = repository;
        _logger = repository.DbContext.Logger;
        _queryBody = new QueryBody(repository);
        _sqlBuilder = new QueryableSqlBuilder(_queryBody);
    }

    protected Queryable(QueryBody queryBody)
    {
        _queryBody = queryBody;
        _repository = queryBody.Repository;
        _sqlBuilder = new QueryableSqlBuilder(_queryBody);
        _logger = _repository.DbContext.Logger;
    }

    #region ==List==

    public Task<IList<dynamic>> ToListDynamic()
    {
        return ToList<dynamic>();
    }

    public async Task<IList<TResult>> ToList<TResult>()
    {
        var sql = _sqlBuilder.BuildListSql(out IQueryParameters parameters);
        _logger.Write("ToList", sql);
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
        _logger.Write("ToReader", sql);
        return _repository.ExecuteReader(sql, parameters.ToDynamicParameters(), _queryBody.Uow);
    }

    #endregion

    #region ==Pagination==

    public Task<IList<dynamic>> ToPaginationDynamic()
    {
        return ToPagination<dynamic>(null);
    }

    public Task<IList<dynamic>> ToPaginationDynamic(Paging paging)
    {
        return ToPagination<dynamic>(paging);
    }

    public Task<IList<TResult>> ToPagination<TResult>()
    {
        return ToPagination<TResult>(null);
    }

    public async Task<IList<TResult>> ToPagination<TResult>(Paging paging)
    {
        if (paging == null)
            _queryBody.SetLimit(1, 15);
        else
            _queryBody.SetLimit(paging.Skip, paging.Size);

        var sql = _sqlBuilder.BuildPaginationSql(out IQueryParameters parameters);
        _logger.Write("ToPagination", sql);

        var task = _repository.Query<TResult>(sql, parameters.ToDynamicParameters(), _queryBody.Uow);

        if (paging != null && paging.QueryCount)
        {
            paging.TotalCount = await ToCount();
        }

        return (await task).ToList();
    }

    public async Task<PagingQueryResultModel<TResult>> ToPaginationResult<TResult>(Paging paging)
    {
        var rows = await ToPagination<TResult>(paging);
        var result = new PagingQueryResultModel<TResult>();
        var resultBody = new PagingQueryResultBody<TResult>(rows, paging.TotalCount);
        return result.Success(resultBody);
    }

    public string ToPaginationSql(Paging paging)
    {
        if (paging == null)
            _queryBody.SetLimit(1, 15);
        else
            _queryBody.SetLimit(paging.Skip, paging.Size);

        return _sqlBuilder.BuildPaginationSql(out _);
    }

    public string ToPaginationSql(Paging paging, out IQueryParameters parameters)
    {
        if (paging == null)
            _queryBody.SetLimit(1, 15);
        else
            _queryBody.SetLimit(paging.Skip, paging.Size);

        return _sqlBuilder.BuildPaginationSql(out parameters);
    }

    public string ToPaginationSql(Paging paging, IQueryParameters parameters)
    {
        if (paging == null)
            _queryBody.SetLimit(1, 15);
        else
            _queryBody.SetLimit(paging.Skip, paging.Size);

        return _sqlBuilder.BuildPaginationSql(parameters);
    }

    public string ToPaginationSqlNotUseParameters(Paging paging)
    {
        if (paging == null)
            _queryBody.SetLimit(1, 15);
        else
            _queryBody.SetLimit(paging.Skip, paging.Size);

        return _sqlBuilder.BuildPaginationSqlNotUseParameters();
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

    #region ==Count==

    public Task<long> ToCount()
    {
        var sql = _sqlBuilder.BuildCountSql(out IQueryParameters parameters);
        _logger.Write("ToCount", sql);
        return _repository.ExecuteScalar<long>(sql, parameters.ToDynamicParameters(), _queryBody.Uow);
    }

    public string ToCountSql()
    {
        return _sqlBuilder.BuildCountSql(out _);
    }

    public string ToCountSql(out IQueryParameters parameters)
    {
        return _sqlBuilder.BuildCountSql(out parameters);
    }

    public string ToCountSql(IQueryParameters parameters)
    {
        return _sqlBuilder.BuildCountSql(parameters);
    }

    public string ToCountSqlNotUseParameters()
    {
        return _sqlBuilder.BuildCountSqlNotUseParameters();
    }

    #endregion

    #region ==Exists==

    public async Task<bool> ToExists()
    {
        var sql = _sqlBuilder.BuildExistsSql(out IQueryParameters parameters);
        _logger.Write("ToExists", sql);
        return await _repository.ExecuteScalar<int>(sql, parameters.ToDynamicParameters(), _queryBody.Uow) > 0;
    }

    public string ToExistsSql()
    {
        return _sqlBuilder.BuildExistsSql(out _);
    }

    public string ToExistsSql(out IQueryParameters parameters)
    {
        return _sqlBuilder.BuildExistsSql(out parameters);
    }

    public string ToExistsSql(IQueryParameters parameters)
    {
        return _sqlBuilder.BuildExistsSql(parameters);
    }

    public string ToExistsSqlNotUseParameters()
    {
        return _sqlBuilder.BuildExistsSqlNotUseParameters();
    }

    #endregion

    #region ==Max==

    protected Task<TResult> ToMax<TResult>(LambdaExpression expression)
    {
        return ExecuteFunction<TResult>("Max", expression);
    }

    public string MaxSql(LambdaExpression expression)
    {
        _queryBody.SetFunctionSelect(expression, "Max");
        return _sqlBuilder.BuildFunctionSql(out _);
    }

    public string MaxSql(LambdaExpression expression, out IQueryParameters parameters)
    {
        _queryBody.SetFunctionSelect(expression, "Max");
        return _sqlBuilder.BuildFunctionSql(out parameters);
    }

    public string MaxSql(LambdaExpression expression, IQueryParameters parameters)
    {
        _queryBody.SetFunctionSelect(expression, "Max");
        return _sqlBuilder.BuildFunctionSql(parameters);
    }

    public string MaxSqlNotUseParameters(LambdaExpression expression)
    {
        _queryBody.SetFunctionSelect(expression, "Max");
        return _sqlBuilder.BuildFunctionSqlNotUseParameters();
    }

    #endregion

    #region ==Min==

    protected Task<TResult> ToMin<TResult>(LambdaExpression expression)
    {
        return ExecuteFunction<TResult>("Min", expression);
    }

    public string MinSql(LambdaExpression expression)
    {
        _queryBody.SetFunctionSelect(expression, "Min");
        return _sqlBuilder.BuildFunctionSql(out _);
    }

    public string MinSql(LambdaExpression expression, out IQueryParameters parameters)
    {
        _queryBody.SetFunctionSelect(expression, "Min");
        return _sqlBuilder.BuildFunctionSql(out parameters);
    }

    public string MinSql(LambdaExpression expression, IQueryParameters parameters)
    {
        _queryBody.SetFunctionSelect(expression, "Min");
        return _sqlBuilder.BuildFunctionSql(parameters);
    }

    public string MinSqlNotUseParameters(LambdaExpression expression)
    {
        _queryBody.SetFunctionSelect(expression, "Min");
        return _sqlBuilder.BuildFunctionSqlNotUseParameters();
    }

    #endregion

    #region ==Sum==

    protected Task<TResult> ToSum<TResult>(LambdaExpression expression)
    {
        return ExecuteFunction<TResult>("Sum", expression);
    }

    public string SumSql(LambdaExpression expression)
    {
        _queryBody.SetFunctionSelect(expression, "Sum");
        return _sqlBuilder.BuildFunctionSql(out _);
    }

    public string SumSql(LambdaExpression expression, out IQueryParameters parameters)
    {
        _queryBody.SetFunctionSelect(expression, "Sum");
        return _sqlBuilder.BuildFunctionSql(out parameters);
    }

    public string SumSql(LambdaExpression expression, IQueryParameters parameters)
    {
        _queryBody.SetFunctionSelect(expression, "Sum");
        return _sqlBuilder.BuildFunctionSql(parameters);
    }

    public string SumSqlNotUseParameters(LambdaExpression expression)
    {
        _queryBody.SetFunctionSelect(expression, "Sum");
        return _sqlBuilder.BuildFunctionSqlNotUseParameters();
    }

    #endregion

    #region ==Avg==

    protected Task<TResult> ToAvg<TResult>(LambdaExpression expression)
    {
        return ExecuteFunction<TResult>("Avg", expression);
    }

    public string AvgSql(LambdaExpression expression)
    {
        _queryBody.SetFunctionSelect(expression, "Avg");
        return _sqlBuilder.BuildFunctionSql(out _);
    }

    public string AvgSql(LambdaExpression expression, out IQueryParameters parameters)
    {
        _queryBody.SetFunctionSelect(expression, "Avg");
        return _sqlBuilder.BuildFunctionSql(out parameters);
    }

    public string AvgSql(LambdaExpression expression, IQueryParameters parameters)
    {
        _queryBody.SetFunctionSelect(expression, "Avg");
        return _sqlBuilder.BuildFunctionSql(parameters);
    }

    public string AvgSqlNotUseParameters(LambdaExpression expression)
    {
        _queryBody.SetFunctionSelect(expression, "Avg");
        return _sqlBuilder.BuildFunctionSqlNotUseParameters();
    }

    #endregion

    #region ==Function==

    private Task<TResult> ExecuteFunction<TResult>(string functionName, LambdaExpression expression)
    {
        _queryBody.SetFunctionSelect(expression, functionName);
        var sql = _sqlBuilder.BuildFunctionSql(out IQueryParameters parameters);
        _logger.Write(functionName, sql);
        return _repository.ExecuteScalar<TResult>(sql, parameters.ToDynamicParameters(), _queryBody.Uow);
    }

    #endregion
}