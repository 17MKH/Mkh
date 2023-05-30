using System;
using Mkh.Data.Abstractions.Annotations;
using Mkh.Data.Abstractions.Entities;

namespace Data.Common.Test.Domain.Article
{
    public class ArticleEntity : EntityBaseSoftDelete
    {
        [Column("分类编号")]
        public int CategoryId { get; set; }

        [Length(300)]
        [Column("标题")]
        public string Title { get; set; }

        [Length(0)]
        [Column("内容")]
        public string Content { get; set; }

        [Column("是否发布", null, null, "IsPublished")]
        public bool Published { get; set; }

        [Column("发布日期")]
        public DateTime? PublishedTime { get; set; }

        [Column("价格")]
        public decimal Price { get; set; }
    }
}
