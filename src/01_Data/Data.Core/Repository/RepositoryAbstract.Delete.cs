using System.Threading.Tasks;
using Mkh.Data.Abstractions;

namespace Mkh.Data.Core.Repository
{
    /// <summary>
    /// 删除
    /// </summary>
    public abstract partial class RepositoryAbstract<TEntity>
    {
        public Task<bool> Delete(dynamic id, IUnitOfWork uow = null)
        {
            return Delete(id, null, uow);
        }

        /// <summary>
        /// 删除实体，自定义表名称
        /// </summary>
        /// <param name="id">实体ID</param>
        /// <param name="tableName">自定义表名称</param>
        /// <param name="uow">工作单元</param>
        /// <returns></returns>
        protected async Task<bool> Delete(dynamic id, string tableName, IUnitOfWork uow = null)
        {
            var dynParams = GetIdParameter(id);
            var sql = _sql.GetDeleteSingle(tableName);
            var result = await Execute(sql, dynParams, uow) > 0;
            return result;
        }
    }
}
