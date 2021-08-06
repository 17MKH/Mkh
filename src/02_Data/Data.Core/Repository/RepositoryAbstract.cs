using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Entities;
using Mkh.Data.Abstractions.Logger;

namespace Mkh.Data.Core.Repository
{
    public abstract partial class RepositoryAbstract<TEntity> : IRepository<TEntity> where TEntity : IEntity, new()
    {
        #region ==字段==

        private IEntitySqlDescriptor _sql;
        private IDbAdapter _adapter;
        private DbLogger _logger;

        #endregion

        #region ==属性==

        public IDbContext DbContext { get; private set; }

        public IUnitOfWork Uow { get; private set; }

        public IEntityDescriptor EntityDescriptor { get; private set; }

        internal IDbTransaction Transaction => Uow?.Transaction;

        public IDbConnection Conn
        {
            get
            {
                //先从事务中获取连接
                if (Transaction != null)
                    return Transaction.Connection;

                return DbContext.NewConnection();
            }
        }

        #endregion

        #region ==初始化==

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="context"></param>
        internal void Init(IDbContext context)
        {
            DbContext = context;
            EntityDescriptor = context.EntityDescriptors.First(m => m.EntityType == typeof(TEntity));
            _sql = EntityDescriptor.SqlDescriptor;
            _adapter = context.Adapter;
            _logger = context.Logger;
        }

        #endregion

        public void BindingUow(IUnitOfWork uow)
        {
            Uow = uow;
        }

        #region ==数据操作方法，对Dapper进行的二次封装，这些方法只能在仓储内访问==

        #region ==Execute==

        /// <summary>
        /// 执行一个命令并返回受影响的行数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="uow">工作单元</param>
        /// <returns></returns>
        public Task<int> Execute(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var conn = OpenConn(uow, out IDbTransaction tran);
            return conn.ExecuteAsync(sql, param, tran, commandType: commandType);
        }

        #endregion

        #region ==ExecuteScalar==

        /// <summary>
        /// 执行一个命令并返回单个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="uow"></param>
        /// <returns></returns>
        public Task<T> ExecuteScalar<T>(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var conn = OpenConn(uow, out IDbTransaction tran);
            return conn.ExecuteScalarAsync<T>(sql, param, tran, commandType: commandType);
        }

        #endregion

        #region ==ExecuteReader==

        /// <summary>
        /// 执行SQL语句并返回IDataReader
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="uow"></param>
        /// <returns></returns>
        public Task<IDataReader> ExecuteReader(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var conn = OpenConn(uow, out IDbTransaction tran);
            return conn.ExecuteReaderAsync(sql, param, tran, commandType: commandType);
        }

        #endregion

        #region ==QueryFirstOrDefault==

        /// <summary>
        /// 查询第一条数据，不存在返回默认值
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="uow"></param>
        /// <returns></returns>
        public Task<dynamic> QueryFirstOrDefault(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var conn = OpenConn(uow, out IDbTransaction tran);
            return conn.QueryFirstOrDefaultAsync(sql, param, tran, commandType: commandType);
        }

        /// <summary>
        /// 查询第一条数据并返回指定类型，不存在返回默认值
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="uow"></param>
        /// <returns></returns>
        public Task<T> QueryFirstOrDefault<T>(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var conn = OpenConn(uow, out IDbTransaction tran);
            return conn.QueryFirstOrDefaultAsync<T>(sql, param, tran, commandType: commandType);
        }

        #endregion

        #region ==QuerySingleOrDefault==

        /// <summary>
        /// 查询单条记录，不存在返回默认值，如果存在多条记录则抛出异常
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="uow"></param>
        /// <returns></returns>
        public Task<dynamic> QuerySingleOrDefault(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var conn = OpenConn(uow, out IDbTransaction tran);
            return conn.QuerySingleOrDefaultAsync(sql, param, tran, commandType: commandType);
        }

        /// <summary>
        /// 查询单条记录并返回指定类型，不存在返回默认值，如果存在多条记录则抛出异常
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="uow"></param>
        /// <returns></returns>
        public Task<T> QuerySingleOrDefault<T>(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var conn = OpenConn(uow, out IDbTransaction tran);
            return conn.QuerySingleOrDefaultAsync<T>(sql, param, tran, commandType: commandType);
        }

        #endregion

        #region ==QueryMultiple==

        /// <summary>
        /// 查询多条结果
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="uow"></param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public Task<SqlMapper.GridReader> QueryMultiple(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var conn = OpenConn(uow, out IDbTransaction tran);
            return conn.QueryMultipleAsync(sql, param, tran, commandType: commandType);
        }

        #endregion

        #region ==Query==

        /// <summary>
        /// 查询结果集返回动态类型
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="uow"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public Task<IEnumerable<dynamic>> Query(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var conn = OpenConn(uow, out IDbTransaction tran);
            return conn.QueryAsync(sql, param, tran, commandType: commandType);
        }

        /// <summary>
        /// 查询数据列表并返回指定类型
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="uow"></param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public Task<IEnumerable<T>> Query<T>(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var conn = OpenConn(uow, out IDbTransaction tran);
            return conn.QueryAsync<T>(sql, param, tran, commandType: commandType);
        }

        #endregion

        #endregion

        #region ==私有方法==

        private IDbConnection OpenConn(IUnitOfWork uow, out IDbTransaction transaction)
        {
            if (uow != null)
            {
                transaction = uow.Transaction;
                return uow.Transaction.Connection;
            }
            //先从事务中获取连接
            if (Transaction != null)
            {
                transaction = Transaction;
                return Transaction.Connection;
            }

            transaction = null;
            return DbContext.NewConnection();
        }

        /// <summary>
        /// 获取主键参数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private DynamicParameters GetIdParameter(dynamic id)
        {
            PrimaryKeyValidate(id);

            var dynParams = new DynamicParameters();
            dynParams.Add(_adapter.AppendParameter("Id"), id);
            return dynParams;
        }

        /// <summary>
        /// 主键验证
        /// </summary>
        /// <param name="id"></param>
        private void PrimaryKeyValidate(dynamic id)
        {
            var primaryKey = EntityDescriptor.PrimaryKey;
            if (primaryKey.IsNo)
                throw new ArgumentException("该实体没有主键，无法使用该方法~");

            //验证id有效性
            if ((primaryKey.IsInt || primaryKey.IsLong) && id < 1)
            {
                throw new ArgumentException("数值类型主键不能小于1~");
            }
            if (primaryKey.IsString && id == null)
            {
                throw new ArgumentException("字符串类型主键不能为null~");
            }
            if (primaryKey.IsGuid && id == Guid.Empty)
            {
                throw new ArgumentException("Guid类型主键不能为空~");
            }
        }

        #endregion
    }
}
