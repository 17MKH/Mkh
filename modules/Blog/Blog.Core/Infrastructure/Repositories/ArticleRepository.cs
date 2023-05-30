using Blog.Core.Domain.Article;
using Mkh.Data.Core.Repository;

namespace Mkh.Mod.Blog.Core.Infrastructure.Repositories
{
    public class ArticleRepository : RepositoryAbstract<ArticleEntity>, IArticleRepository
    {
    }
}
