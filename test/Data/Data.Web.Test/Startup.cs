using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Data.Common.Test;
using Data.Common.Test.Infrastructure;
using Data.Common.Test.Service;
using Mkh.Data.Abstractions;

namespace Data.Web.Test
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //自定义账户信息解析器
            services.AddSingleton<IOperatorResolver, CustomAccountResolver>();

            AddMySql(services);

            services.AddControllers();
        }

        void AddMySql(IServiceCollection services)
        {
            var connString = "Database=blog;Data Source=localhost;Port=3306;User Id=root;Password=root;Charset=utf8;SslMode=None;allowPublicKeyRetrieval=true;";

            services
                .AddMkhDb<BlogDbContext>(options =>
                {
                    //开启日志
                    options.Log = true;
                })
                .UseMySql(connString)
                .AddRepositoriesFromAssembly(typeof(BlogDbContext).Assembly)
                .AddTransactionAttribute(typeof(IArticleService), typeof(ArticleService));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
