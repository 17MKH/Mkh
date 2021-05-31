using System;
using Mkh.Data.Abstractions.Annotations;
using Mkh.Data.Abstractions.Entities;

namespace Blog.Core.Domain.Article
{
    [Table("Article")]
    public class ArticleEntity : EntityBaseSoftDelete, ITenant
    {
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Length(300)]
        public string Title { get; set; }
    }
}
