using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mkh.Data.Abstractions;

namespace Mkh.Host.Web.BackgroundServices
{
    /// <summary>
    /// 创建分表后台服务
    /// </summary>
    public class CreateTableBackgroundService : BackgroundService
    {
        private readonly IEnumerable<IDbContext> _dbContexts;
        private readonly ILogger<CreateTableBackgroundService> _logger;

        public CreateTableBackgroundService(IEnumerable<IDbContext> dbContexts, ILogger<CreateTableBackgroundService> logger)
        {
            _dbContexts = dbContexts;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_dbContexts.Any(m => m.EntityDescriptors.Any(n => n.IsSharding)))
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Begin:Create next table");

                    try
                    {
                        foreach (var dbContext in _dbContexts)
                        {
                            dbContext.CodeFirstProvider.CreateNextTable();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Create next table error:{@ex}", ex);
                    }

                    _logger.LogInformation("End:Create next table");

                    await Task.Delay(6000000, stoppingToken);
                }
            }
        }
    }
}
