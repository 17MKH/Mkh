using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Options;
using Mkh.Utils.Helpers;

namespace Mkh.Data.Core
{
    public abstract class CodeFirstProviderAbstract : ICodeFirstProvider
    {
        protected readonly CodeFirstOptions Options;
        protected readonly IDbContext Context;
        protected readonly IServiceCollection Service;
        private readonly ILogger<CodeFirstProviderAbstract> _logger;

        protected CodeFirstProviderAbstract(CodeFirstOptions options, IDbContext context, IServiceCollection service)
        {
            Options = options;
            Context = context;
            Service = service;
            _logger = service.BuildServiceProvider().GetService<ILogger<CodeFirstProviderAbstract>>();
        }

        public abstract bool CreateDatabase();

        public abstract void CreateTable();

        public virtual void InitData(IRepositoryManager repositoryManager)
        {
            if (!Options.InitData)
                return;


            if (Options.InitDataFilePath.IsNull() || !File.Exists(Options.InitDataFilePath))
            {
                _logger.LogDebug("初始化数据文件不存在");
                return;
            }

            _logger.LogDebug("开始初始化数据");

            var jsonHelper = new JsonHelper();

            using var jsonReader = new StreamReader(Options.InitDataFilePath, Encoding.UTF8);
            var str = jsonReader.ReadToEnd();

            using var doc = JsonDocument.Parse(str);
            var properties = doc.RootElement.EnumerateObject();
            if (properties.Any())
            {
                foreach (var property in properties)
                {
                    var entityDescriptor = Context.EntityDescriptors.FirstOrDefault(m => m.Name.EqualsIgnoreCase(property.Name));
                    if (entityDescriptor != null)
                    {
                        var list = (IList)jsonHelper.Deserialize(property.Value.ToString(),
                            typeof(List<>).MakeGenericType(entityDescriptor.EntityType));

                        var repositoryDescriptor = Context.RepositoryDescriptors.FirstOrDefault(m => m.EntityType == entityDescriptor.EntityType);

                        var repository = (IRepository)Service.BuildServiceProvider()
                            .GetService(repositoryDescriptor!.InterfaceType);

                        var tasks = new List<Task>();
                        foreach (var item in list)
                        {
                            tasks.Add(repository.Add(item));
                        }

                        Task.WaitAll(tasks.ToArray());
                    }
                }
            }
        }
    }
}
