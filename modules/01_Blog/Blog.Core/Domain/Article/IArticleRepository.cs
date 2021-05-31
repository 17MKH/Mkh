using Mkh.Data.Abstractions;

namespace Blog.Core.Domain.Article
{
    /// <summary>
    /// 文章仓储
    /// </summary>
    public interface IArticleRepository : IRepository<ArticleEntity>
    {
    }
}
