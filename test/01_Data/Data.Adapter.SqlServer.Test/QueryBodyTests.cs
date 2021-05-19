using System;
using System.Linq;
using System.Linq.Expressions;
using Data.Common.Test.Domain.Article;
using Microsoft.Extensions.DependencyInjection;
using Mkh.Data.Core.Internal.QueryStructure;
using Xunit;
using Xunit.Abstractions;

namespace Data.Adapter.SqlServer.Test
{
    public class QueryBodyTests : DbContextTests
    {
        private readonly QueryBody _queryBody;

        public QueryBodyTests(ITestOutputHelper output) : base(output)
        {
            _queryBody = new QueryBody(_serviceProvider.GetService<IArticleRepository>());
            _queryBody.Joins.Add(new QueryJoin(_dbContext.EntityDescriptors.FirstOrDefault(m => m.TableName == "Article"), "T1"));
        }

        [Fact]
        public void GetColumnNameTest()
        {
            Expression<Func<ArticleEntity, int>> exp = m => m.Id;
            var columnName = _queryBody.GetColumnName(exp.Body);
            Assert.Equal("[Id]", columnName);
        }
    }
}
