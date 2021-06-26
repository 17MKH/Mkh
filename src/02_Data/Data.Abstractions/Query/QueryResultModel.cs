using System.Collections.Generic;
using System.Threading.Tasks;
using Mkh.Data.Abstractions.Pagination;
using Mkh.Utils.Models;

namespace Mkh.Data.Abstractions.Query
{
    /// <summary>
    /// 查询结果模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryResultModel<T>
    {
        /// <summary>
        /// 总数
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// 数据集
        /// </summary>
        public IList<T> Rows { get; set; }

        /// <summary>
        /// 扩展数据
        /// </summary>
        public object Data { get; set; }

        public QueryResultModel(IList<T> rows, long total)
        {
            Rows = rows;
            Total = total;
        }
    }

    public class QueryResultModel
    {
        /// <summary>
        /// 返回查询模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rows"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        public static async Task<IResultModel> Success<T>(Task<IList<T>> rows, Paging paging)
        {
            return ResultModel.Success(new QueryResultModel<T>(await rows, paging.TotalCount));
        }
    }
}
