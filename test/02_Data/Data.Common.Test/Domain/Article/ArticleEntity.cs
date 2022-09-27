using System;
using Mkh.Data.Abstractions.Annotations;
using Mkh.Data.Abstractions.Entities;

namespace Data.Common.Test.Domain.Article
{
    /// <summary>
    ///  分表策略: 使用ShardingField特性，按指定发布日期（PublishedTime），为分表字段
    /// </summary>
    [Sharding(ShardingPolicy.Month)]
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
        [ShardingField]//分表字段
        public DateTime? PublishedTime { get; set; }

        [Column("价格")]
        public decimal Price { get; set; }
    }
}
