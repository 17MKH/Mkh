using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Data.Common.Test.Domain.Article;
using Data.Common.Test.Domain.Category;
using Microsoft.Extensions.DependencyInjection;
using Mkh.Data.Abstractions.Extensions;
using Mkh.Data.Abstractions.Queryable;
using Mkh.Data.Core.Internal;
using Mkh.Data.Core.Internal.QueryStructure;
using Xunit;
using Xunit.Abstractions;

namespace Data.Adapter.SqlServer.Test
{
    /*
     * 多表连接表达式解析测试
     */
    public class MultiTableExpressionResolverTests : DbContextTests
    {
        private readonly QueryBody _queryBody;

        public MultiTableExpressionResolverTests(ITestOutputHelper output) : base(output)
        {
            _queryBody = new QueryBody(_serviceProvider.GetService<IArticleRepository>());
            _queryBody.Joins.Add(new QueryJoin(_dbContext.EntityDescriptors.FirstOrDefault(m => m.TableName == "Article"), "T1"));
            Expression<Func<ArticleEntity, CategoryEntity, bool>> exp = (m, n) => m.CategoryId == n.Id;
            _queryBody.Joins.Add(new QueryJoin(_dbContext.EntityDescriptors.FirstOrDefault(m => m.TableName == "MyCategory"), "T2", JoinType.Left, exp));
        }

        /// <summary>
        /// 解析表达式
        /// </summary>
        [Fact]
        public void ResolveExpressionTest()
        {
            var parameters = new QueryParameters();
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, bool>> exp = m => m.T1.Id > 10 && m.T2.Name == "mkh";
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("T1.[Id] > @P1 AND T2.[Name] = @P2", sql);
            Assert.Equal(2, parameters.Count);
            Assert.Equal("mkh", parameters[1].Value);
        }

        /// <summary>
        /// 解析表达式
        /// </summary>
        [Fact]
        public void ResolveExpressionTest1()
        {
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, bool>> exp = m => m.T1.Id > 10 && m.T1.Id < 20 && m.T2.Name == "123";

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("T1.[Id] > @P1 AND T1.[Id] < @P2 AND T2.[Name] = @P3", sql);
            Assert.Equal(3, parameters.Count);
        }

        /// <summary>
        /// 解析测试字符串的Length属性
        /// </summary>
        [Fact]
        public void ResolveExpressionTest3()
        {
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, bool>> exp = m => m.T1.Title.Length >= 10;

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("LEN(T1.[Title]) >= @P1", sql);
            Assert.Equal(1, parameters.Count);
        }

        /// <summary>
        /// 解析测试Substring函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest4()
        {
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, bool>> exp = m => m.T1.Title.Substring(3, 3) == "10";

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("SUBSTR(T1.[Title],4,3) = @P1", sql);
            Assert.Equal(1, parameters.Count);
        }

        /// <summary>
        /// 解析测试Replace函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest5()
        {
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, bool>> exp = m => m.T1.Title.Replace("a", "b") == "10";

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("REPLACE(T1.[Title],'a','b') = @P1", sql);
            Assert.Equal(1, parameters.Count);
        }

        /// <summary>
        /// 解析测试ToUpper和ToLower函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest6()
        {
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, bool>> exp = m => m.T1.Title.ToUpper() == "10" || m.T2.Name.ToLower() == "10";

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("UPPER(T1.[Title]) = @P1 OR LOWER(T2.[Name]) = @P2", sql);
            Assert.Equal(2, parameters.Count);
        }

        /// <summary>
        /// 解析测试常量参数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest7()
        {
            var id = 10;
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, bool>> exp = m => m.T1.Id > id;

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("T1.[Id] > @P1", sql);
            Assert.Equal(1, parameters.Count);
            Assert.Equal(10, parameters[0].Value);
        }

        /// <summary>
        /// 解析测试函数调用
        /// </summary>
        [Fact]
        public void ResolveExpressionTest8()
        {
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, bool>> exp = m => m.T1.Id > GetId();

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("T1.[Id] > @P1", sql);
            Assert.Equal(1, parameters.Count);
            Assert.Equal(10, parameters[0].Value);
        }

        private int GetId()
        {
            return 10;
        }

        /// <summary>
        /// 解析测试布尔类型
        /// </summary>
        [Fact]
        public void ResolveExpressionTest9()
        {
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, bool>> exp = m => m.T1.Published == true;

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("T1.[IsPublished] = @P1", sql);
            Assert.Equal(1, parameters.Count);
            Assert.Equal(true, parameters[0].Value);
        }

        /// <summary>
        /// 解析测试布尔类型
        /// </summary>
        [Fact]
        public void ResolveExpressionTest10()
        {
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, bool>> exp = m => m.T1.Published;

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("T1.[IsPublished] = @P1", sql);
            Assert.Equal(1, parameters.Count);
            Assert.Equal("1", parameters[0].Value);
        }

        /// <summary>
        /// 解析测试布尔类型
        /// </summary>
        [Fact]
        public void ResolveExpressionTest11()
        {
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, bool>> exp = m => m.T1.Published && m.T2.Id > 10;

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("T1.[IsPublished] = @P1 AND T2.[Id] > @P2", sql);
            Assert.Equal(2, parameters.Count);
            Assert.Equal(10, parameters[1].Value);
        }

        /// <summary>
        /// 解析测试布尔类型
        /// </summary>
        [Fact]
        public void ResolveExpressionTest12()
        {
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, bool>> exp = m => !m.T1.Published && m.T2.Id > 10;

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("T1.[IsPublished] = @P1 AND T2.[Id] > @P2", sql);
            Assert.Equal(2, parameters.Count);
            Assert.Equal(10, parameters[1].Value);
        }

        /// <summary>
        /// 解析集合类型的Contains函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest13()
        {
            var ids = new List<int> { 10, 15 };

            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, bool>> exp = m => ids.Contains(m.T1.Id);

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("T1.[Id] IN (10,15)", sql);
        }

        /// <summary>
        /// 解析数组类型的Contains函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest14()
        {
            var ids = new[] { 10, 15 };

            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, bool>> exp = m => ids.Contains(m.T1.Id);

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("T1.[Id] IN (10,15)", sql);
        }

        /// <summary>
        /// 解析字符串类型的Contains函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest15()
        {
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, bool>> exp = m => m.T1.Title.Contains("mkh");

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("T1.[Title] LIKE @P1", sql);
            Assert.Equal("%mkh%", parameters[0].Value);
        }

        /// <summary>
        /// 解析StartsWith函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest16()
        {
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, bool>> exp = m => m.T2.Name.StartsWith("mkh");

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("T2.[Name] LIKE @P1", sql);
            Assert.Equal("mkh%", parameters[0].Value);
        }

        /// <summary>
        /// 解析StartsWith函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest17()
        {
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, bool>> exp = m => m.T1.Title.EndsWith("mkh");

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("T1.[Title] LIKE @P1", sql);
            Assert.Equal("%mkh", parameters[0].Value);
        }

        /// <summary>
        /// 解析Equals函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest18()
        {
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, bool>> exp = m => m.T1.Title.Equals("mkh");

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("T1.[Title] = @P1", sql);
            Assert.Equal("mkh", parameters[0].Value);
        }

        /// <summary>
        /// 解析数组类型的NotContains函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest19()
        {
            var ids = new[] { 10, 15 };

            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, bool>> exp = m => ids.NotContains(m.T1.Id);

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("T1.[Id] NOT IN (10,15)", sql);
        }

        /// <summary>
        /// 解析集合类型的NotContains函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest20()
        {
            var ids = new List<int> { 10, 15 };

            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, bool>> exp = m => ids.NotContains(m.T1.Id);

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("T1.[Id] NOT IN (10,15)", sql);
        }

        /// <summary>
        /// 解析集合类型的Contains函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest21()
        {
            var ids = new List<int> { 10, 15 };

            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, bool>> exp = m => ids.AsEnumerable().Contains(m.T1.Id);

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("T1.[Id] IN (10,15)", sql);
        }

        #region ==解析表==

        //解析表
        [Fact]
        public void ResolveFormTest1()
        {
            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();

            ExpressionResolver.ResolveFrom(_queryBody, sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("[Article] AS T1 WITH (NOLOCK) LEFT JOIN [MyCategory] AS T2 WITH (NOLOCK) ON T1.[CategoryId] = T2.[Id]", sql);
        }

        #endregion

        #region ==解析过滤条件==

        [Fact]
        public void ResolveWhereTest()
        {
            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();
            ExpressionResolver.ResolveWhere(_queryBody, sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("WHERE T1.[Deleted] = 0", sql);
        }

        [Fact]
        public void ResolveWhereTest1()
        {
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, bool>> exp = m => m.T1.Id > 10;

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();
            _queryBody.SetWhere(exp);
            ExpressionResolver.ResolveWhere(_queryBody, sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("WHERE T1.[Id] > @P1 AND T1.[Deleted] = 0", sql);
            Assert.Equal(1, parameters.Count);
        }

        #endregion
    }
}
