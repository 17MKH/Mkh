using System;
using System.Linq;
using System.Linq.Expressions;
using Data.Common.Test.Domain.Article;
using Data.Common.Test.Domain.Category;
using Microsoft.Extensions.DependencyInjection;
using Mkh.Data.Abstractions.Queryable;
using Mkh.Data.Core.Internal.QueryStructure;
using Xunit;
using Xunit.Abstractions;

namespace Data.Adapter.Sqlite.Test
{
    /// <summary>
    /// 多表QueryBody测试
    /// </summary>
    public class MultiTableQueryBodyTests : DbContextTests
    {
        private readonly QueryBody _queryBody;

        public MultiTableQueryBodyTests(ITestOutputHelper output) : base(output)
        {
            _queryBody = new QueryBody(_serviceProvider.GetService<IArticleRepository>());
            _queryBody.Joins.Add(new QueryJoin(_dbContext.EntityDescriptors.FirstOrDefault(m => m.TableName == "Article"), "T1"));
            _queryBody.Joins.Add(new QueryJoin(_dbContext.EntityDescriptors.FirstOrDefault(m => m.TableName == "MyCategory"), "T2"));
        }

        [Fact]
        public void GetColumnNameTest()
        {
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, int>> exp1 = m => m.T1.Id;
            var columnName = _queryBody.GetColumnName(exp1.Body);
            Assert.Equal("T1.[Id]", columnName);

            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, string>> exp2 = m => m.T2.Name;
            columnName = _queryBody.GetColumnName(exp2.Body);
            Assert.Equal("T2.[Name]", columnName);
        }
    }
}