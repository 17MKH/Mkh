using System;
using System.Linq.Expressions;
using Data.Common.Test.Domain.Article;
using Xunit;

namespace Data.Common.Test
{
    /// <summary>
    /// 表达式测试
    /// </summary>
    public class ExpressionTests
    {
        [Fact]
        public void ExpressionTypeTest()
        {
            Expression<Func<ArticleEntity, int>> exp = m => m.Id;
            Expression<Func<ArticleEntity, int>> exp1 = m => m.Title.Length;
            Expression<Func<ArticleEntity, object>> exp2 = m => new { m.Id };
            Expression<Func<ArticleEntity, object>> exp3 = m => new { m.Title.Length };
            Expression<Func<ArticleEntity, object>> exp4 = m => new { Title = m.Title.Substring(10) };

            Assert.Equal(ExpressionType.MemberAccess, exp.Body.NodeType);
            Assert.Equal(ExpressionType.MemberAccess, exp1.Body.NodeType);
            Assert.Equal(ExpressionType.New, exp2.Body.NodeType);
            Assert.Equal(ExpressionType.New, exp3.Body.NodeType);
            Assert.Equal(ExpressionType.New, exp4.Body.NodeType);
        }

    }
}
