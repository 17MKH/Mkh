using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Data.Common.Test.Domain.Article;
using Microsoft.Extensions.DependencyInjection;
using Mkh.Data.Abstractions.Extensions;
using Mkh.Data.Core.Internal;
using Mkh.Data.Core.Internal.QueryStructure;
using Xunit;
using Xunit.Abstractions;

namespace Data.Adapter.Sqlite.Test
{
    public class ExpressionResolverTests : DbContextTests
    {
        private readonly QueryBody _queryBody;

        public ExpressionResolverTests(ITestOutputHelper output) : base(output)
        {
            _queryBody = new QueryBody(_serviceProvider.GetService<IArticleRepository>());
            _queryBody.Joins.Add(new QueryJoin(_dbContext.EntityDescriptors.FirstOrDefault(m => m.TableName == "Article"), "T1"));
        }

        /// <summary>
        /// 解析表达式
        /// </summary>
        [Fact]
        public void ResolveExpressionTest()
        {
            Expression<Func<ArticleEntity, bool>> exp = m => m.Id > 10;

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("[Id] > @P1", sql);
            Assert.Equal(1, parameters.Count);
        }

        /// <summary>
        /// 解析表达式
        /// </summary>
        [Fact]
        public void ResolveExpressionTest1()
        {
            Expression<Func<ArticleEntity, bool>> exp = m => m.Id > 10 && m.Id < 20 && m.Title == "123";

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("[Id] > @P1 AND [Id] < @P2 AND [Title] = @P3", sql);
            Assert.Equal(3, parameters.Count);
        }

        /// <summary>
        /// 解析测试字符串的Length属性
        /// </summary>
        [Fact]
        public void ResolveExpressionTest3()
        {
            Expression<Func<ArticleEntity, bool>> exp = m => m.Title.Length >= 10;

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("LENGTH([Title]) >= @P1", sql);
            Assert.Equal(1, parameters.Count);
        }

        /// <summary>
        /// 解析测试Substring函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest4()
        {
            Expression<Func<ArticleEntity, bool>> exp = m => m.Title.Substring(3, 3) == "10";

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("SUBSTR([Title],4,3) = @P1", sql);
            Assert.Equal(1, parameters.Count);
        }

        /// <summary>
        /// 解析测试Replace函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest5()
        {
            Expression<Func<ArticleEntity, bool>> exp = m => m.Title.Replace("a", "b") == "10";

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("REPLACE([Title],'a','b') = @P1", sql);
            Assert.Equal(1, parameters.Count);
        }

        /// <summary>
        /// 解析测试ToUpper和ToLower函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest6()
        {
            Expression<Func<ArticleEntity, bool>> exp = m => m.Title.ToUpper() == "10" || m.Title.ToLower() == "10";

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("UPPER([Title]) = @P1 OR LOWER([Title]) = @P2", sql);
            Assert.Equal(2, parameters.Count);
        }

        /// <summary>
        /// 解析测试常量参数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest7()
        {
            var id = 10;
            Expression<Func<ArticleEntity, bool>> exp = m => m.Id > id;

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("[Id] > @P1", sql);
            Assert.Equal(1, parameters.Count);
            Assert.Equal(10, parameters[0].Value);
        }

        /// <summary>
        /// 解析测试函数调用
        /// </summary>
        [Fact]
        public void ResolveExpressionTest8()
        {
            Expression<Func<ArticleEntity, bool>> exp = m => m.Id > GetId();

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("[Id] > @P1", sql);
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
            Expression<Func<ArticleEntity, bool>> exp = m => m.Published == true;

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("[IsPublished] = @P1", sql);
            Assert.Equal(1, parameters.Count);
            Assert.Equal(true, parameters[0].Value);
        }

        /// <summary>
        /// 解析测试布尔类型
        /// </summary>
        [Fact]
        public void ResolveExpressionTest10()
        {
            Expression<Func<ArticleEntity, bool>> exp = m => m.Published;

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("[IsPublished] = @P1", sql);
            Assert.Equal(1, parameters.Count);
            Assert.Equal("1", parameters[0].Value);
        }

        /// <summary>
        /// 解析测试布尔类型
        /// </summary>
        [Fact]
        public void ResolveExpressionTest11()
        {
            Expression<Func<ArticleEntity, bool>> exp = m => m.Published && m.Id > 10;

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("[IsPublished] = @P1 AND [Id] > @P2", sql);
            Assert.Equal(2, parameters.Count);
            Assert.Equal(10, parameters[1].Value);
        }

        /// <summary>
        /// 解析测试布尔类型
        /// </summary>
        [Fact]
        public void ResolveExpressionTest12()
        {
            Expression<Func<ArticleEntity, bool>> exp = m => !m.Published && m.Id > 10;

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("[IsPublished] = @P1 AND [Id] > @P2", sql);
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

            Expression<Func<ArticleEntity, bool>> exp = m => ids.Contains(m.Id);

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("[Id] IN (10,15)", sql);
        }

        /// <summary>
        /// 解析数组类型的Contains函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest14()
        {
            var ids = new[] { 10, 15 };

            Expression<Func<ArticleEntity, bool>> exp = m => ids.Contains(m.Id);

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("[Id] IN (10,15)", sql);
        }

        /// <summary>
        /// 解析字符串类型的Contains函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest15()
        {
            Expression<Func<ArticleEntity, bool>> exp = m => m.Title.Contains("mkh");

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("[Title] LIKE @P1", sql);
            Assert.Equal("%mkh%", parameters[0].Value);
        }

        /// <summary>
        /// 解析StartsWith函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest16()
        {
            Expression<Func<ArticleEntity, bool>> exp = m => m.Title.StartsWith("mkh");

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("[Title] LIKE @P1", sql);
            Assert.Equal("mkh%", parameters[0].Value);
        }

        /// <summary>
        /// 解析StartsWith函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest17()
        {
            Expression<Func<ArticleEntity, bool>> exp = m => m.Title.EndsWith("mkh");

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("[Title] LIKE @P1", sql);
            Assert.Equal("%mkh", parameters[0].Value);
        }

        /// <summary>
        /// 解析Equals函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest18()
        {
            Expression<Func<ArticleEntity, bool>> exp = m => m.Title.Equals("mkh");

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("[Title] = @P1", sql);
            Assert.Equal("mkh", parameters[0].Value);
        }

        /// <summary>
        /// 解析数组类型的NotContains函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest19()
        {
            var ids = new[] { 10, 15 };

            Expression<Func<ArticleEntity, bool>> exp = m => ids.NotContains(m.Id);

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("[Id] NOT IN (10,15)", sql);
        }

        /// <summary>
        /// 解析集合类型的NotContains函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest20()
        {
            var ids = new List<int> { 10, 15 };

            Expression<Func<ArticleEntity, bool>> exp = m => ids.NotContains(m.Id);

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("[Id] NOT IN (10,15)", sql);
        }

        /// <summary>
        /// 解析集合类型的Contains函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest21()
        {
            var ids = new List<int> { 10, 15 };

            Expression<Func<ArticleEntity, bool>> exp = m => ids.AsEnumerable().Contains(m.Id);

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("[Id] IN (10,15)", sql);
        }

        /// <summary>
        /// 解析日期类型
        /// </summary>
        [Fact]
        public void ResolveExpressionTest22()
        {
            var date = new DateTime(2022, 05, 09);
            var dto = new { date };
            Expression<Func<ArticleEntity, bool>> exp = m => m.PublishedTime >= dto.date.Date;

            var parameters = new QueryParameters();
            var sql = ExpressionResolver.Resolve(_queryBody, exp, parameters);

            Assert.Equal("[PublishedTime] >= @P1", sql);
        }

        #region ==解析表==

        //解析表
        [Fact]
        public void ResolveFormTest()
        {
            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();
            ExpressionResolver.ResolveFrom(_queryBody, sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("[Article]", sql);
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

            Assert.Equal("WHERE [Deleted] = 0", sql);
        }

        [Fact]
        public void ResolveWhereTest1()
        {
            Expression<Func<ArticleEntity, bool>> exp = m => m.Id > 10;

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();
            _queryBody.SetWhere(exp);
            ExpressionResolver.ResolveWhere(_queryBody, sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("WHERE [Id] > @P1 AND [Deleted] = 0", sql);
            Assert.Equal(1, parameters.Count);
        }

        #endregion
    }
}
