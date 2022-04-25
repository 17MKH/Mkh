using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mkh.Logging.Abstractions.Providers;

namespace Mkh.Logging.Core.DefaultProviders
{
    internal class AuditLogHandler : IAuditLogHandler
    {
        private readonly ILogger<AuditLogHandler> _logger;

        public AuditLogHandler(ILogger<AuditLogHandler> logger)
        {
            _logger = logger;
        }

        public Task<bool> Write(AuditLogModel model)
        {
            _logger.LogInformation("Audit Log：{@model}", model);

            return Task.FromResult(true);
        }
    }
}
