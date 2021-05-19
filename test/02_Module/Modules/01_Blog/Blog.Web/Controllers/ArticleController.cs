using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Mkh.Module.Blog.Domain.Article;

namespace Mkh.Module.Blog.Web.Controllers
{
    [ApiController]
    public class ArticleController : ControllerBase
    {
        public IActionResult Query()
        {
            var articles = new List<ArticleEntity>
            {
                new ArticleEntity {Id = 1, Title = "测试文章1"},
                new ArticleEntity {Id = 2, Title = "测试文章2"}
            };

            return new JsonResult(articles);
        }
    }
}
