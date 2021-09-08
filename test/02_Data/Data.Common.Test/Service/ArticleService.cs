using System.Threading.Tasks;
using Data.Common.Test.Domain.Article;
using Data.Common.Test.Domain.Category;
using Mkh.Data.Abstractions.Annotations;

namespace Data.Common.Test.Service
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ArticleService(IArticleRepository articleRepository, ICategoryRepository categoryRepository)
        {
            _articleRepository = articleRepository;
            _categoryRepository = categoryRepository;
        }

        [Transaction]
        public async Task<bool> Add()
        {
            var article = new ArticleEntity
            {
                Title = "test",
                Content = "test"
            };

            await _articleRepository.Add(article);

            var category = new CategoryEntity
            {
                Name = null,
            };

            await _categoryRepository.Add(category);

            return true;
        }
    }
}
