using Data.Common.Test.Domain.Article;
using Data.Common.Test.Domain.Category;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Data.Adapter.MySql.Test
{
    /*
     * 多表分组查询测试
     */
    public class MultiTableGroupingTests : BaseTest
    {
        private readonly IArticleRepository _repository;

        public MultiTableGroupingTests(ITestOutputHelper output) : base(output)
        {
            _repository = _serviceProvider.GetService<IArticleRepository>();
        }

        [Fact]
        public void Test()
        {
            var sql = _repository.Find().LeftJoin<CategoryEntity>(m => m.T1.CategoryId == m.T2.Id)
                .GroupBy(m => new { name = m.T2.Name.Substring(1, 3) })
                .Select(m => new
                {
                    Sum = m.Sum(x => x.T1.Id),
                    Count = m.Count(),
                    name1 = m.Key.name
                })
                .ToListSql();

            Assert.Equal("SELECT SUM(T1.`Id`) AS `Sum`,COUNT(0) AS `Count`,SUBSTR(T2.`Name`,2,3) AS `name1` FROM `Article` AS T1 LEFT JOIN `MyCategory` AS T2 ON T1.`CategoryId` = T2.`Id` WHERE T1.`Deleted` = 0 GROUP BY SUBSTR(T2.`Name`,2,3)", sql);
        }

        [Fact]
        public void Test1()
        {
            var sql = _repository.Find().LeftJoin<CategoryEntity>(m => m.T1.CategoryId == m.T2.Id)
                .GroupBy(m => new
                {
                    m.T2.Name
                })
                .Having(m => m.Sum(x => x.T1.Id) > 5)
                .OrderBy(m => m.Sum(x => x.T1.Id))
                .OrderByDescending(m => m.Key.Name.Substring(2))
                .Select(m => new
                {
                    Sum = m.Sum(x => x.T1.Id),
                    Name = m.Key.Name.Substring(5)
                })
                .ToListSql();

            Assert.Equal("SELECT SUM(T1.`Id`) AS `Sum`,SUBSTR(T2.`Name`,6) AS `Name` FROM `Article` AS T1 LEFT JOIN `MyCategory` AS T2 ON T1.`CategoryId` = T2.`Id` WHERE T1.`Deleted` = 0 GROUP BY T2.`Name` HAVING SUM(T1.`Id`) > @P1 ORDER BY SUM(T1.`Id`) ASC, SUBSTR(T2.`Name`,3) DESC", sql);
        }

        [Fact]
        public void Test2()
        {
            var sql = _repository.Find().LeftJoin<CategoryEntity>(m => m.T1.CategoryId == m.T2.Id)
                .GroupBy(m => new
                {
                    m.T2.Name.Length
                })
                .Having(m => m.Sum(x => x.T1.Id) > 5)
                .OrderBy(m => m.Sum(x => x.T1.Id))
                .Select(m => new
                {
                    Sum = m.Sum(x => x.T1.Id),
                    m.Key.Length
                })
                .ToListSql();

            Assert.Equal("SELECT SUM(T1.`Id`) AS `Sum`,CHAR_LENGTH(T2.`Name`) AS `Length` FROM `Article` AS T1 LEFT JOIN `MyCategory` AS T2 ON T1.`CategoryId` = T2.`Id` WHERE T1.`Deleted` = 0 GROUP BY CHAR_LENGTH(T2.`Name`) HAVING SUM(T1.`Id`) > @P1 ORDER BY SUM(T1.`Id`) ASC", sql);
        }
    }
}
