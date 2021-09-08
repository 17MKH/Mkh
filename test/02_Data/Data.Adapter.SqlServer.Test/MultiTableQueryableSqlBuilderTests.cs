using System;
using System.Linq;
using System.Linq.Expressions;
using Data.Common.Test.Domain.Article;
using Data.Common.Test.Domain.Category;
using Microsoft.Extensions.DependencyInjection;
using Mkh.Data.Abstractions.Pagination;
using Mkh.Data.Abstractions.Queryable;
using Mkh.Data.Core.Internal.QueryStructure;
using Mkh.Data.Core.SqlBuilder;
using Xunit;
using Xunit.Abstractions;

namespace Data.Adapter.SqlServer.Test
{
    /// <summary>
    /// 多表测试
    /// </summary>
    public class MultiTableQueryableSqlBuilderTests : DbContextTests
    {
        private readonly QueryableSqlBuilder _builder;
        private readonly QueryBody _queryBody;
        public MultiTableQueryableSqlBuilderTests(ITestOutputHelper output) : base(output)
        {
            _queryBody = new QueryBody(_serviceProvider.GetService<IArticleRepository>());
            _queryBody.Joins.Add(new QueryJoin(_dbContext.EntityDescriptors.FirstOrDefault(m => m.TableName == "Article"), "T1"));
            Expression<Func<ArticleEntity, CategoryEntity, bool>> exp = (m, n) => m.CategoryId == n.Id;
            _queryBody.Joins.Add(new QueryJoin(_dbContext.EntityDescriptors.FirstOrDefault(m => m.TableName == "MyCategory"), "T2", JoinType.Left, exp));
            _builder = new QueryableSqlBuilder(_queryBody);
        }

        #region ==解析排序==

        //解析排序测试
        [Fact]
        public void ResolveSortTest()
        {
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, string>> exp = m => m.T1.Title;
            _queryBody.SetSort(exp, SortType.Asc);
            Assert.Equal(" ORDER BY T1.[Title] ASC", _builder.ResolveSort());
        }

        //解析排序测试
        [Fact]
        public void ResolveSortForSqlTest()
        {
            _queryBody.SetSort("T1.Title", SortType.Asc);
            Assert.Equal(" ORDER BY T1.Title ASC", _builder.ResolveSort());
        }

        //解析排序，针对字符串的Substring函数
        [Fact]
        public void ResolveSortForSubstringTest()
        {
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, string>> exp = m => m.T1.Title.Substring(1);
            _queryBody.SetSort(exp, SortType.Asc);
            Assert.Equal(" ORDER BY SUBSTR(T1.[Title],2) ASC", _builder.ResolveSort());
        }

        //解析排序，针对字符串的Length属性
        [Fact]
        public void ResolveSortForLengthTest()
        {
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, int>> exp = m => m.T1.Title.Length;
            _queryBody.SetSort(exp, SortType.Asc);
            Assert.Equal(" ORDER BY LEN(T1.[Title]) ASC", _builder.ResolveSort());
        }

        //解析排序，针对日期格式化函数
        [Fact]
        public void ResolveSortForDatetimeFormatTest()
        {
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, string>> exp = m => m.T1.CreatedTime.ToString("yyyy-MM-dd");
            _queryBody.SetSort(exp, SortType.Asc);
            Assert.Equal(" ORDER BY FORMAT(T1.[CreatedTime],'yyyy-MM-dd') ASC", _builder.ResolveSort());
        }

        //解析排序，针对字符串的Replace函数
        [Fact]
        public void ResolveSortForReplaceTest()
        {
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, string>> exp = m => m.T1.Title.Replace("a", "c");
            _queryBody.SetSort(exp, SortType.Asc);
            Assert.Equal(" ORDER BY REPLACE(T1.[Title],'a','c') ASC", _builder.ResolveSort());
        }

        //解析排序，针对字符串的ToLower函数
        [Fact]
        public void ResolveSortForToLowerTest()
        {
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, string>> exp = m => m.T1.Title.ToLower();
            _queryBody.SetSort(exp, SortType.Asc);
            Assert.Equal(" ORDER BY LOWER(T1.[Title]) ASC", _builder.ResolveSort());
        }

        //解析排序，针对字符串的ToUpper函数
        [Fact]
        public void ResolveSortForToUpperTest()
        {
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, string>> exp = m => m.T1.Title.ToUpper();
            _queryBody.SetSort(exp, SortType.Asc);
            Assert.Equal(" ORDER BY UPPER(T1.[Title]) ASC", _builder.ResolveSort());
        }

        //解析排序，针对匿名函数
        [Fact]
        public void ResolveSortForNewTest()
        {
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, dynamic>> exp = m => new { m.T1.Id, m.T2.Name };
            _queryBody.SetSort(exp, SortType.Asc);
            var sql = _builder.ResolveSort();
            Assert.Equal(" ORDER BY T1.[Id] ASC, T2.[Name] ASC", sql);
        }

        [Fact]
        public void ResolveSortForNewTest1()
        {
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, dynamic>> exp1 = m => new { m.T1.Title.Length, Name = m.T2.Name.Substring(2, 2) };
            _queryBody.SetSort(exp1, SortType.Desc);
            Assert.Equal(" ORDER BY LEN(T1.[Title]) DESC, SUBSTR(T2.[Name],3,2) DESC", _builder.ResolveSort());
        }

        #endregion

        #region ==解析排除列==

        //解析排除列
        [Fact]
        public void ResolveSelectExcludeColumnsTest()
        {
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, string>> exp = m => m.T1.Content;
            _queryBody.SetSelectExclude(exp);
            var columns = _builder.ResolveSelectExcludeColumns();

            Assert.Single(columns);

            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, dynamic>> exp1 = m => new { m.T1.Id, m.T2.Name };
            _queryBody.SetSelectExclude(exp1);
            columns = _builder.ResolveSelectExcludeColumns();

            Assert.Equal(2, columns.Count);
        }

        #endregion

        #region ==解析选择列==

        //解析自定类型的选择列
        [Fact]
        public void ResolveSelectTest()
        {
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, dynamic>> exp4 = m => new { m.T1.Title, m.T2.Name };

            _queryBody.SetSelect(exp4);
            var sql = _builder.ResolveSelect();

            Assert.Equal("T1.[Title] AS [Title],T2.[Name] AS [Name]", sql);

            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, dynamic>> exp5 = m => new { m.T1.Title, m.T2 };

            _queryBody.SetSelect(exp5);
            sql = _builder.ResolveSelect();

            Assert.Equal("T1.[Title] AS [Title],T2.[Id] AS [Id],T2.[Name] AS [Name],T2.[CreatedBy] AS [CreatedBy],T2.[Creator] AS [Creator],T2.[CreatedTime] AS [CreatedTime],T2.[ModifiedBy] AS [ModifiedBy],T2.[Modifier] AS [Modifier],T2.[ModifiedTime] AS [ModifiedTime]", sql);

            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, dynamic>> exp6 = m => new { m.T1.Title, Name = m.T2.Name.Substring(3, 2) };

            _queryBody.SetSelect(exp6);
            sql = _builder.ResolveSelect();
            Assert.Equal("T1.[Title] AS [Title],SUBSTR(T2.[Name],4,2) AS [Name]", sql);

            //排除列
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, dynamic>> exp7 = m => new { m.T1.Id, m.T1.Title, m.T2 };
            Expression<Func<IQueryableJoins<ArticleEntity, CategoryEntity>, dynamic>> exp8 = m => new { m.T1.Title, m.T2.Name };

            _queryBody.SetSelect(exp7);
            _queryBody.SetSelectExclude(exp8);
            sql = _builder.ResolveSelect();

            Assert.Equal("T1.[Id] AS [Id],T2.[Id] AS [Id],T2.[CreatedBy] AS [CreatedBy],T2.[Creator] AS [Creator],T2.[CreatedTime] AS [CreatedTime],T2.[ModifiedBy] AS [ModifiedBy],T2.[Modifier] AS [Modifier],T2.[ModifiedTime] AS [ModifiedTime]", sql);
        }

        #endregion

    }
}
