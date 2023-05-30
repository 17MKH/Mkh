using Data.Common.Test.Domain.Article;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Data.Adapter.SqlServer.Test
{
    public class GroupingTests : BaseTest
    {
        private readonly IArticleRepository _repository;

        public GroupingTests(ITestOutputHelper output) : base(output)
        {
            _repository = _serviceProvider.GetService<IArticleRepository>();
        }

        [Fact]
        public void ToListTest1()
        {
            var query = _repository.Find().GroupBy(m => new { m.Title, m.Deleted })
                .Select(m => new
                {
                    Sum = m.Sum(n => n.Id),
                    m.Key.Title
                });

            var sql = query.ToListSql();
            Assert.Equal("SELECT SUM([Id]) AS [Sum],[Title] AS [Title] FROM [Article] WITH (NOLOCK) WHERE [Deleted] = 0 GROUP BY [Title], [Deleted]", sql);
        }

        [Fact]
        public void ToListTest2()
        {
            var query = _repository.Find()
                .GroupBy(m => new { m.Deleted })
                .Having(m => m.Sum(x => x.Id) > 3)
                .Select(m => new { Sum = m.Sum(n => n.Id) });

            var sql = query.ToListSql();
            Assert.Equal("SELECT SUM([Id]) AS [Sum] FROM [Article] WITH (NOLOCK) WHERE [Deleted] = 0 GROUP BY [Deleted] HAVING SUM([Id]) > @P1", sql);

            sql = query.ToListSqlNotUseParameters();
            Assert.Equal("SELECT SUM([Id]) AS [Sum] FROM [Article] WITH (NOLOCK) WHERE [Deleted] = 0 GROUP BY [Deleted] HAVING SUM([Id]) > 3", sql);
        }

        [Fact]
        public void ToListTest3()
        {
            var query = _repository.Find()
                .Where(m => m.Id > 5)
                .GroupBy(m => new { m.Deleted })
                .Having(m => m.Sum(x => x.Id) > 3)
                .OrderBy(m => m.Sum(x => x.Id))
                .Select(m => new
                {
                    Sum = m.Sum(n => n.Id),
                });

            var sql = query.ToListSql();
            Assert.Equal("SELECT SUM([Id]) AS [Sum] FROM [Article] WITH (NOLOCK) WHERE [Id] > @P1 AND [Deleted] = 0 GROUP BY [Deleted] HAVING SUM([Id]) > @P2 ORDER BY SUM([Id]) ASC", sql);

            sql = query.ToListSqlNotUseParameters();
            Assert.Equal("SELECT SUM([Id]) AS [Sum] FROM [Article] WITH (NOLOCK) WHERE [Id] > 5 AND [Deleted] = 0 GROUP BY [Deleted] HAVING SUM([Id]) > 3 ORDER BY SUM([Id]) ASC", sql);
        }

        [Fact]
        public void ToListTest4()
        {
            var sql = _repository.Find().GroupBy(m => new { m.Deleted })
                .Having(m => m.Sum(x => x.Id) > 3)
                .OrderBy(m => m.Key.Deleted)
                .Select(m => new
                {
                    Sum = m.Sum(n => n.Id),
                })
                .ToListSql();

            Assert.Equal("SELECT SUM([Id]) AS [Sum] FROM [Article] WITH (NOLOCK) WHERE [Deleted] = 0 GROUP BY [Deleted] HAVING SUM([Id]) > @P1 ORDER BY [Deleted] ASC", sql);
        }

        [Fact]
        public void ToListTest5()
        {
            var sql = _repository.Find().GroupBy(m => new { m.Title })
                .Having(m => m.Sum(x => x.Id) > 3)
                .OrderBy(m => m.Key.Title.Substring(3))
                .Select(m => new
                {
                    Sum = m.Sum(n => n.Id),
                })
                .ToListSql();

            Assert.Equal("SELECT SUM([Id]) AS [Sum] FROM [Article] WITH (NOLOCK) WHERE [Deleted] = 0 GROUP BY [Title] HAVING SUM([Id]) > @P1 ORDER BY SUBSTR([Title],4) ASC", sql);
        }

        [Fact]
        public void ToFirstTest1()
        {
            var query = _repository.Find().GroupBy(m => new { m.Title, m.Deleted })
                .Select(m => new
                {
                    Sum = m.Sum(n => n.Id),
                    m.Key.Title
                })
                .OrderBy(m => m.Key.Title);

            var sql = query.ToFirstSql();
            Assert.Equal("SELECT SUM([Id]) AS [Sum],[Title] AS [Title] FROM [Article] WITH (NOLOCK) WHERE [Deleted] = 0 GROUP BY [Title], [Deleted] ORDER BY [Title] ASC OFFSET 0 ROW FETCH NEXT 1 ROW ONLY", sql);
        }

        [Fact]
        public void ToFirstTest2()
        {
            var query = _repository.Find()
                .GroupBy(m => new { m.Deleted })
                .Having(m => m.Sum(x => x.Id) > 3)
                .Select(m => new { Sum = m.Sum(n => n.Id) })
                .OrderBy(m => m.Key.Deleted);

            var sql = query.ToFirstSql();
            Assert.Equal("SELECT SUM([Id]) AS [Sum] FROM [Article] WITH (NOLOCK) WHERE [Deleted] = 0 GROUP BY [Deleted] HAVING SUM([Id]) > @P1 ORDER BY [Deleted] ASC OFFSET 0 ROW FETCH NEXT 1 ROW ONLY", sql);

            sql = query.ToFirstSqlNotUseParameters();
            Assert.Equal("SELECT SUM([Id]) AS [Sum] FROM [Article] WITH (NOLOCK) WHERE [Deleted] = 0 GROUP BY [Deleted] HAVING SUM([Id]) > 3 ORDER BY [Deleted] ASC OFFSET 0 ROW FETCH NEXT 1 ROW ONLY", sql);
        }
    }
}
