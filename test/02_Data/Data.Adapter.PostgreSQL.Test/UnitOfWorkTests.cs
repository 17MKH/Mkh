using Data.Common.Test.Domain.Article;
using Data.Common.Test.Domain.Category;
using Xunit;
using Xunit.Abstractions;
namespace Data.Adapter.PostgreSQL.Test;

public class UnitOfWorkTests : BaseTest
{
    public UnitOfWorkTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async void SaveChangesTest()
    {
        await ClearTable();

        using var uow = _dbContext.NewUnitOfWork();

        var category = new CategoryEntity
        {
            Name = ".Net"
        };

        await _categoryRepository.Add(category, uow);

        var article = new ArticleEntity
        {
            CategoryId = category.Id,
            Title = "工作单元测试",
            Content = "工作单元测试"
        };

        await _articleRepository.Add(article, uow);

        uow.SaveChanges();

        var article1 = await _articleRepository.Get(article.Id);

        Assert.Equal(article1.Title, article.Title);
    }

    [Fact]
    public async void RollbackTest()
    {
        await ClearTable();

        using var uow = _dbContext.NewUnitOfWork();
        try
        {
            var category = new CategoryEntity
            {
                Name = null
            };

            await _categoryRepository.Add(category, uow);

            var article = new ArticleEntity
            {
                CategoryId = category.Id,
                Title = "工作单元测试",
                Content = "工作单元测试"
            };

            await _articleRepository.Add(article, uow);

            uow.SaveChanges();
        }
        catch
        {
            uow.Rollback();
            Assert.True(true);
        }
    }
}
