using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Common.Test.Domain.Article;
using Microsoft.Extensions.DependencyInjection;
using Mkh.Data.Abstractions.Extensions;
using Mkh.Data.Abstractions.Pagination;
using Xunit;
using Xunit.Abstractions;

namespace Data.Adapter.MySql.Test
{
    public class RepositoryTest : BaseTest
    {
        private const string Select =
            "SELECT `Id` AS `Id`,`CategoryId` AS `CategoryId`,`Title` AS `Title`,`Content` AS `Content`,`IsPublished` AS `Published`,`PublishedTime` AS `PublishedTime`,`Price` AS `Price`,`Deleted` AS `Deleted`,`DeletedBy` AS `DeletedBy`,`Deleter` AS `Deleter`,`DeletedTime` AS `DeletedTime`,`CreatedBy` AS `CreatedBy`,`Creator` AS `Creator`,`CreatedTime` AS `CreatedTime`,`ModifiedBy` AS `ModifiedBy`,`Modifier` AS `Modifier`,`ModifiedTime` AS `ModifiedTime` FROM `Article`";

        public RepositoryTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async void AddTest()
        {
            await ClearTable();

            var article = new ArticleEntity
            {
                Title = "test",
                Content = "test"
            };

            var b = await _articleRepository.Add(article);
            /*INSERT INTO `Article` (`CategoryId`,`Title`,`Content`,`Published`,`PublishedTime`,`TimeSpan`,`CreatedBy`,
            `Creator`,`CreatedTime`,`ModifiedBy`,`Modifier`,`ModifiedTime`) VALUES(@CategoryId,@Title,@Content,@Published,
            @PublishedTime,@TimeSpan,@CreatedBy,@Creator,@CreatedTime,@ModifiedBy,@Modifier,@ModifiedTime);*/

            Assert.True(b);
            Assert.True(article.Id > 0);

            var article1 = await _articleRepository.Get(article.Id);

            Assert.Equal(article.Title, article1.Title);
            Assert.Equal("OLDLI", article1.Creator);
        }

        [Fact]
        public async void DeleteTest()
        {
            var article = new ArticleEntity
            {
                Title = "test",
                Content = "test"
            };

            await _articleRepository.Add(article);

            Assert.True(article.Id > 0);

            var result = await _articleRepository.Delete(article.Id);
            //DELETE FROM `Article`  WHERE `Id`=@Id;

            Assert.True(result);

            var article1 = await _articleRepository.Get(article.Id);

            Assert.Null(article1);

            //条件删除
            var sql = _articleRepository.Find(m => m.Id > 10).ToDeleteSql();
            Assert.Equal("DELETE FROM `Article`  WHERE `Id` > @P1 AND `Deleted` = 0", sql);

            sql = _articleRepository.Find(m => m.Id > 10).ToDeleteSqlNotUseParameters();
            Assert.Equal("DELETE FROM `Article`  WHERE `Id` > 10 AND `Deleted` = 0", sql);
        }

        [Fact]
        public async void UpdateTest()
        {
            var article = new ArticleEntity
            {
                Title = "test",
                Content = "test"
            };

            await _articleRepository.Add(article);

            var article1 = await _articleRepository.Get(article.Id);
            article1.Title = "修改标题";

            var result = await _articleRepository.Update(article1);
            /*
             * SELECT `Id` AS `Id`,`CategoryId` AS `CategoryId`,`Title` AS `Title`,`Content` AS `Content`,`Published` AS `Published`,
             * `PublishedTime` AS `PublishedTime`,`TimeSpan` AS `TimeSpan`,`CreatedBy` AS `CreatedBy`,`Creator` AS `Creator`,
             * `CreatedTime` AS `CreatedTime`,`ModifiedBy` AS `ModifiedBy`,`Modifier` AS `Modifier`,`ModifiedTime` AS `ModifiedTime`
             * FROM `Article`  WHERE `Id`=@Id 
             */

            Assert.True(result);

            var article2 = await _articleRepository.Get(article1.Id);

            Assert.Equal(article1.Title, article2.Title);
            Assert.Equal("OLDLI", article1.Modifier);

            //条件批量更新
            result = await _articleRepository.Find(m => m.Id > 10).ToUpdate(m => new ArticleEntity { Title = "条件更新" });

            Assert.True(result);

            var sql = _articleRepository.Find(m => m.Id > 10).ToUpdateSql(m => new ArticleEntity { Title = "条件更新" });
            Assert.Equal("UPDATE `Article` SET `Title` = @P1,`ModifiedBy` = @P2,`Modifier` = @P3,`ModifiedTime` = @P4 WHERE `Id` > @P5 AND `Deleted` = 0;", sql);
        }

        [Fact]
        public async void SoftDeleteTest()
        {
            var article = new ArticleEntity
            {
                Title = "test",
                Content = "test"
            };

            await _articleRepository.Add(article);

            Assert.True(article.Id > 0);

            var result = await _articleRepository.SoftDelete(article.Id);
            //DELETE FROM `Article`  WHERE `Id`=@Id;

            Assert.True(result);

            var article1 = await _articleRepository.Get(article.Id);

            Assert.Null(article1);

            var sql = _articleRepository.Find(m => m.Id > 0).ToSoftDeleteSql();
            Assert.Equal("UPDATE `Article` SET `Deleted` = 1,`DeletedTime` = @P1,`DeletedBy` = @P2,`Deleter` = @P3 WHERE `Id` > @P4 AND `Deleted` = 0", sql);
        }

        [Fact]
        public async void ExistsTest()
        {
            var article = new ArticleEntity
            {
                Title = "test",
                Content = "test"
            };

            await _articleRepository.Add(article);

            var exists = await _articleRepository.Exists(article.Id);
            //SELECT 1 FROM `Article` WHERE `Id`=@Id AND `Deleted`=0  LIMIT 1

            Assert.True(exists);

            exists = await _articleRepository.Exists(100000);
            Assert.False(exists);

            exists = await _articleRepository.Find(m => m.Id == 1).ToExists();
            Assert.True(exists);

            exists = await _articleRepository.Find(m => m.Id == 1000).ToExists();
            Assert.False(exists);
        }

        private async Task ClearAndAdd(int count = 10)
        {
            await ClearTable();

            for (int i = 1; i <= count; i++)
            {
                var article = new ArticleEntity
                {
                    Title = i < count / 2 ? "test" + i : "mkh" + i,
                    Content = "test"
                };

                await _articleRepository.Add(article);
            }
        }

        [Fact]
        public async void ListTest()
        {
            await ClearAndAdd();

            var list = await _articleRepository.Find().ToList();

            Assert.Equal(10, list.Count);

            list = await _articleRepository.Find(m => m.Id > 5).OrderBy(m => m.Id).ToList();
            Assert.Equal("mkh10", list[4].Title);

            list = await _articleRepository.Find(m => m.Id == 7).ToList();
            Assert.Single(list);
            Assert.Equal("mkh7", list.First().Title);

            list = await _articleRepository.Find(m => m.Title.Contains("9")).ToList();
            Assert.Single(list);

            list = await _articleRepository.Find(m => m.Title.StartsWith("mkh")).ToList();
            Assert.Equal(6, list.Count);

            list = await _articleRepository.Find(m => m.Title.EndsWith("9") || m.Title.EndsWith("1")).ToList();
            Assert.Equal(2, list.Count);

            var ids = new List<int> { 3, 5, 9 };
            list = await _articleRepository.Find(m => ids.Contains(m.Id)).ToList();
            Assert.Equal(3, list.Count);
            Assert.Equal("mkh5", list[1].Title);

            list = await _articleRepository.Find(m => ids.NotContains(m.Id)).ToList();
            Assert.Equal(7, list.Count);
            Assert.Equal("test1", list[0].Title);

            var sql = _articleRepository.Find(m => m.Id > 10).ToListSql();
            Assert.Equal(Select + " WHERE `Id` > @P1 AND `Deleted` = 0", sql);

            sql = _articleRepository.Find(m => m.Id > 10).ToListSqlNotUseParameters();
            Assert.Equal(Select + " WHERE `Id` > 10 AND `Deleted` = 0", sql);
        }

        [Fact]
        public async void PaginationTest()
        {
            await ClearAndAdd(20);

            var list = await _articleRepository.Find().ToPagination();

            Assert.Equal(15, list.Count);

            var paging = new Paging(2, 10);
            list = await _articleRepository.Find().ToPagination(paging);

            Assert.Equal(20, paging.TotalCount);
            Assert.Equal("mkh11", list[0].Title);

            list = await _articleRepository.Find(m => m.Id > 5).ToPagination(new Paging(2, 3));
            Assert.Equal("test9", list[0].Title);
        }

        [Fact]
        public void FirstTest()
        {
            var sql = _articleRepository.Find(m => m.Title == "test3").ToFirstSql();

            Assert.Equal(Select + " WHERE `Title` = @P1 AND `Deleted` = 0 LIMIT 1", sql);
        }

        [Fact]
        public void NotContainsTest()
        {
            var ids = new List<int>();
            var sql = _articleRepository.Find(m => ids.NotContains(m.Id)).ToListSql();

            Assert.Equal(Select + " WHERE `Deleted` = 0", sql);
        }

        [Fact]
        public async void FunctionTest()
        {
            await ClearAndAdd();

            var maxId = await _articleRepository.Find().ToMax(m => m.Id);

            Assert.Equal(10, maxId);

            maxId = await _articleRepository.Find(m => m.Id < 8).ToMax(m => m.Id);

            Assert.Equal(7, maxId);

            var minId = await _articleRepository.Find(m => m.Id > 5).ToMin(m => m.Id);

            Assert.Equal(6, minId);

            var avg = await _articleRepository.Find(m => m.Id > 5 && m.Id < 10).ToAvg<decimal>(m => m.Id);

            Assert.Equal(7.5M, avg);

            var sum = await _articleRepository.Find(m => m.Id > 5 && m.Id < 10).ToSum(m => m.Id);

            Assert.Equal(30, sum);
        }

        [Fact]
        public async void CopyTest()
        {
            await ClearAndAdd();

            var query = _articleRepository.Find(m => m.Id > 5);
            var list = await query.ToList();
            Assert.Equal(5, list.Count);

            var query1 = query.Copy();
            var list1 = await query1.ToList();
            Assert.Equal(5, list1.Count);
        }
    }
}
