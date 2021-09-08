using Data.Common.Test.Domain.Article;
using Mkh.Data.Core.Repository;

namespace Data.Common.Test.Infrastructure.Repositories
{
    public class ArticleRepository : RepositoryAbstract<ArticleEntity>, IArticleRepository
    {
    }
}
