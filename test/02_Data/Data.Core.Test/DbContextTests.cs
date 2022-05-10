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
            var connString = "";
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
    }
}
