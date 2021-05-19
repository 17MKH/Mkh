using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Data.Common.Test.Domain.Article;
using Microsoft.Extensions.DependencyInjection;
using Mkh.Data.Abstractions.Pagination;
using Mkh.Data.Core.Internal.QueryStructure;
using Mkh.Data.Core.SqlBuilder;
using Xunit;
using Xunit.Abstractions;

namespace Data.Adapter.SqlServer.Test
{
    public class QueryableSqlBuilderTests : DbContextTests
    {
        public QueryableSqlBuilderTests(ITestOutputHelper output) : base(output)
        {
        }

        internal QueryableSqlBuilder CreateBuilder()
        {
            var queryBody = new QueryBody(_serviceProvider.GetService<IArticleRepository>());
            queryBody.Joins.Add(new QueryJoin(_dbContext.EntityDescriptors.FirstOrDefault(m => m.TableName == "Article"), "T1"));
            return new QueryableSqlBuilder(queryBody);
        }

        #region ==解析排序==

        //解析排序测试
        [Fact]
        public void ResolveSortTest()
        {
            var builder = CreateBuilder();

            Expression<Func<ArticleEntity, string>> exp = m => m.Title;
            builder.QueryBody.SetSort(exp, SortType.Asc);
            Assert.Equal(" ORDER BY [Title] ASC", builder.ResolveSort());
        }

        //解析排序测试
        [Fact]
        public void ResolveSortForSqlTest()
        {
            var builder = CreateBuilder();

            builder.QueryBody.SetSort("Title", SortType.Asc);
            Assert.Equal(" ORDER BY Title ASC", builder.ResolveSort());
        }

        //解析排序，针对字符串的Substring函数
        [Fact]
        public void ResolveSortForSubstringTest()
        {
            var builder = CreateBuilder();

            Expression<Func<ArticleEntity, string>> exp = m => m.Title.Substring(1);
            builder.QueryBody.SetSort(exp, SortType.Asc);
            Assert.Equal(" ORDER BY SUBSTR([Title],2) ASC", builder.ResolveSort());
        }

        //解析排序，针对字符串的Length属性
        [Fact]
        public void ResolveSortForLengthTest()
        {
            var builder = CreateBuilder();

            Expression<Func<ArticleEntity, int>> exp = m => m.Title.Length;
            builder.QueryBody.SetSort(exp, SortType.Asc);
            Assert.Equal(" ORDER BY LEN([Title]) ASC", builder.ResolveSort());
        }

        //解析排序，针对日期格式化函数
        [Fact]
        public void ResolveSortForDatetimeFormatTest()
        {
            var builder = CreateBuilder();
            Expression<Func<ArticleEntity, string>> exp = m => m.CreatedTime.ToString("yyyy-MM-dd");
            builder.QueryBody.SetSort(exp, SortType.Asc);
            Assert.Equal(" ORDER BY FORMAT([CreatedTime],'yyyy-MM-dd') ASC", builder.ResolveSort());

            builder = CreateBuilder();
            Expression<Func<ArticleEntity, string>> exp1 = m => m.CreatedTime.ToString("yyyy-MM-dd");
            builder.QueryBody.SetSort(exp1, SortType.Asc);
            Assert.Equal(" ORDER BY FORMAT([CreatedTime],'yyyy-MM-dd') ASC", builder.ResolveSort());
        }

        //解析排序，针对字符串的Replace函数
        [Fact]
        public void ResolveSortForReplaceTest()
        {
            var builder = CreateBuilder();
            Expression<Func<ArticleEntity, string>> exp = m => m.Title.Replace("a", "c");
            builder.QueryBody.SetSort(exp, SortType.Asc);
            Assert.Equal(" ORDER BY REPLACE([Title],'a','c') ASC", builder.ResolveSort());
        }

        //解析排序，针对字符串的ToLower函数
        [Fact]
        public void ResolveSortForToLowerTest()
        {
            var builder = CreateBuilder();
            Expression<Func<ArticleEntity, string>> exp = m => m.Title.ToLower();
            builder.QueryBody.SetSort(exp, SortType.Asc);
            Assert.Equal(" ORDER BY LOWER([Title]) ASC", builder.ResolveSort());
        }

        //解析排序，针对字符串的ToUpper函数
        [Fact]
        public void ResolveSortForToUpperTest()
        {
            var builder = CreateBuilder();
            Expression<Func<ArticleEntity, string>> exp = m => m.Title.ToUpper();
            builder.QueryBody.SetSort(exp, SortType.Asc);
            Assert.Equal(" ORDER BY UPPER([Title]) ASC", builder.ResolveSort());
        }

        //解析排序，针对匿名函数
        [Fact]
        public void ResolveSortForNewTest()
        {
            var builder = CreateBuilder();
            Expression<Func<ArticleEntity, dynamic>> exp = m => new { m.Id, m.Title };
            builder.QueryBody.SetSort(exp, SortType.Asc);
            Assert.Equal(" ORDER BY [Id] ASC, [Title] ASC", builder.ResolveSort());

            builder = CreateBuilder();
            Expression<Func<ArticleEntity, dynamic>> exp1 = m => new { m.Title.Length, Title = m.Title.Substring(2, 2) };
            builder.QueryBody.SetSort(exp1, SortType.Desc);
            Assert.Equal(" ORDER BY LEN([Title]) DESC, SUBSTR([Title],3,2) DESC", builder.ResolveSort());
        }

        #endregion

        #region ==解析排除列==

        //解析排除列
        [Fact]
        public void ResolveSelectExcludeColumnsTest()
        {
            var builder = CreateBuilder();
            Expression<Func<ArticleEntity, string>> exp = m => m.Content;
            builder.QueryBody.SetSelectExclude(exp);
            var columns = builder.ResolveSelectExcludeColumns();

            Assert.Single(columns);


            Expression<Func<ArticleEntity, dynamic>> exp1 = m => new { m.Id, m.Content };
            builder.QueryBody.SetSelectExclude(exp1);
            columns = builder.ResolveSelectExcludeColumns();

            Assert.Equal(2, columns.Count);
        }

        #endregion

        #region ==解析选择列==

        //解析单个完整实体
        [Fact]
        public void ResolveSelectForEntityTest()
        {
            var builder = CreateBuilder();
            var sb = new StringBuilder();
            builder.ResolveSelectForEntity(sb);

            var sql = sb.ToString();

            Assert.Equal("[Id] AS [Id],[CategoryId] AS [CategoryId],[Title] AS [Title],[Content] AS [Content],[IsPublished] AS [Published],[PublishedTime] AS [PublishedTime],[Price] AS [Price],[Deleted] AS [Deleted],[DeletedBy] AS [DeletedBy],[Deleter] AS [Deleter],[DeletedTime] AS [DeletedTime],[CreatedBy] AS [CreatedBy],[Creator] AS [Creator],[CreatedTime] AS [CreatedTime],[ModifiedBy] AS [ModifiedBy],[Modifier] AS [Modifier],[ModifiedTime] AS [ModifiedTime],", sql);
        }

        //解析单个实体并排除指定列
        [Fact]
        public void ResolveSelectForEntityAndExcludeColumnsTest()
        {
            //排除单个列
            var builder = CreateBuilder();
            var sb = new StringBuilder();
            Expression<Func<ArticleEntity, string>> exp = m => m.Content;
            builder.QueryBody.SetSelectExclude(exp);
            var columns = builder.ResolveSelectExcludeColumns();
            builder.ResolveSelectForEntity(sb, 0, columns);

            var sql = sb.ToString();

            Assert.Equal("[Id] AS [Id],[CategoryId] AS [CategoryId],[Title] AS [Title],[IsPublished] AS [Published],[PublishedTime] AS [PublishedTime],[Price] AS [Price],[Deleted] AS [Deleted],[DeletedBy] AS [DeletedBy],[Deleter] AS [Deleter],[DeletedTime] AS [DeletedTime],[CreatedBy] AS [CreatedBy],[Creator] AS [Creator],[CreatedTime] AS [CreatedTime],[ModifiedBy] AS [ModifiedBy],[Modifier] AS [Modifier],[ModifiedTime] AS [ModifiedTime],", sql);

            //排除多个列
            sb.Clear();
            Expression<Func<ArticleEntity, dynamic>> exp1 = m => new { m.CategoryId, m.Content };
            builder.QueryBody.SetSelectExclude(exp1);
            columns = builder.ResolveSelectExcludeColumns();
            builder.ResolveSelectForEntity(sb, 0, columns);

            sql = sb.ToString();

            Assert.Equal("[Id] AS [Id],[Title] AS [Title],[IsPublished] AS [Published],[PublishedTime] AS [PublishedTime],[Price] AS [Price],[Deleted] AS [Deleted],[DeletedBy] AS [DeletedBy],[Deleter] AS [Deleter],[DeletedTime] AS [DeletedTime],[CreatedBy] AS [CreatedBy],[Creator] AS [Creator],[CreatedTime] AS [CreatedTime],[ModifiedBy] AS [ModifiedBy],[Modifier] AS [Modifier],[ModifiedTime] AS [ModifiedTime],", sql);
        }

        //解析自定SQL语句类型选择列
        [Fact]
        public void ResolveSelectForSqlTest()
        {
            var builder = CreateBuilder();
            builder.QueryBody.SetSelect("Title AS 'Title1'");
            var sql = builder.ResolveSelect();

            Assert.Equal("Title AS 'Title1'", sql);
        }

        //解析自定类型的选择列
        [Fact]
        public void ResolveSelectTest()
        {
            var builder = CreateBuilder();
            Expression<Func<ArticleEntity, string>> exp = m => m.Content;
            builder.QueryBody.SetSelect(exp);
            var sql = builder.ResolveSelect();

            Assert.Equal("[Content] AS [Content]", sql);

            Expression<Func<ArticleEntity, dynamic>> exp1 = m => new { m.Id, Len = m.Title.Length };
            builder.QueryBody.SetSelect(exp1);
            sql = builder.ResolveSelect();

            Assert.Equal("[Id] AS [Id],LEN([Title]) AS [Len]", sql);

            Expression<Func<ArticleEntity, dynamic>> exp2 = m => new { m.Id, Title = m.Title.Substring(2), Content = m.Content.Replace("a", "b") };
            builder.QueryBody.SetSelect(exp2);
            sql = builder.ResolveSelect();

            Assert.Equal("[Id] AS [Id],SUBSTR([Title],3) AS [Title],REPLACE([Content],'a','b') AS [Content]", sql);


            Expression<Func<ArticleEntity, dynamic>> exp3 = m => new { Title = m.Title.ToLower(), Content = m.Content.ToUpper() };
            builder.QueryBody.SetSelect(exp3);
            sql = builder.ResolveSelect();

            Assert.Equal("LOWER([Title]) AS [Title],UPPER([Content]) AS [Content]", sql);
        }

        #endregion
    }
}
