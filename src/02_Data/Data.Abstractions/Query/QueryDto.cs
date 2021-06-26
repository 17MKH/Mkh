using System.Linq;
using Mkh.Data.Abstractions.Pagination;

namespace Mkh.Data.Abstractions.Query
{
    /// <summary>
    /// 查询对象
    /// </summary>
    public abstract class QueryDto
    {
        /// <summary>
        /// 分页信息
        /// </summary>
        public QueryPagingDto Page { get; set; } = new();

        /// <summary>
        /// 转换成Paging分页类
        /// </summary>
        public Paging Paging
        {
            get
            {
                var paging = new Paging(Page.Index, Page.Size);

                if (Page.Sort != null && Page.Sort.Any())
                {
                    foreach (var sort in Page.Sort)
                    {
                        paging.OrderBy.Add(new Sort(sort.Field, sort.Type));
                    }
                }

                return paging;
            }
        }
    }
}
