using System;
using Data.Common.Test;
using Data.Common.Test.Domain.Article;
using Data.Common.Test.Domain.Category;
using Data.Common.Test.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mkh.Data.Abstractions;
using Xunit;

namespace Data.Core.Test
{
    public class DbContextTests
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IDbContext _context;
        protected readonly IArticleRepository _articleRepository;
        protected readonly ICategoryRepository _categoryRepository;

        public DbContextTests()
        {
            var connString = "server=localhost;user id=root;password=xxx;port=3306;persistsecurityinfo=True;database=test_sharding;Convert Zero Datetime=True;TreatTinyAsBoolean=true;allowuservariables=True;";

            var services = new ServiceCollection();
            //日志
            services.AddLogging(builder =>
            {
                builder.AddDebug();
            });

            services.AddSingleton<IOperatorResolver, CustomAccountResolver>();

            services
                .AddMkhDb<BlogDbContext>()
                .UseMySql(connString)
                .AddRepositoriesFromAssembly(typeof(BlogDbContext).Assembly)
                //开启代码优先
                .AddCodeFirst(options =>
                {
                    //创建库
                    options.CreateDatabase = true;

                    //更新列
                    options.UpdateColumn = true;

                    options.BeforeCreateDatabase = ctx =>
                    {
                        ctx.Logger.Write("BeforeCreateDatabase", "数据库创建前事件");
                    };
                    options.AfterCreateDatabase = ctx =>
                    {
                        ctx.Logger.Write("AfterCreateDatabase", "数据库创建后事件");
                    };
                    options.BeforeCreateTable = (ctx, entityDescriptor) =>
                    {
                        ctx.Logger.Write("BeforeCreateTable", "表创建前事件，表名称：" + entityDescriptor.TableName);
                    };
                    options.AfterCreateTable = (ctx, entityDescriptor) =>
                    {
                        ctx.Logger.Write("AfterCreateTable", "表创建后事件，表名称：" + entityDescriptor.TableName);
                    };
                })
                .Build();

            _serviceProvider = services.BuildServiceProvider();
            _context = _serviceProvider.GetService<BlogDbContext>();

            _articleRepository = _serviceProvider.GetService<IArticleRepository>();
            _categoryRepository = _serviceProvider.GetService<ICategoryRepository>();
        }

        [Fact]
        public void RepositoryDescriptorsCountTest()
        {
            Assert.Equal(2, _context.EntityDescriptors.Count);
        }

        [Fact]
        public void AccountResolverTest()
        {
            var accountResolverType = _context.AccountResolver.GetType();
            Assert.Equal(typeof(CustomAccountResolver), accountResolverType);
        }

        /// <summary>
        /// 测试分表
        /// ArticleEntity 已经启用分表策略 -> [Sharding(ShardingPolicy.Month)]
        /// </summary>
        [Fact]
        public async void ArticleEntityAddTest()
        {
            var article = new ArticleEntity
            {
                Title = "test",
                Content = "test",
                //该字段为分表字段
                PublishedTime = DateTime.Parse("2022-06-06 00:00:00")
            };

            await _articleRepository.Add(article);

            Assert.True(article.Id > 0);
        }
    }
}
