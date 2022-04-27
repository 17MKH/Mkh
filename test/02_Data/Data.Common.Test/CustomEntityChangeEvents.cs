using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mkh.Data.Abstractions.EntityChangeEvents;

namespace Data.Common.Test
{
    public class CustomEntityChangeEvents : IEntityChangeEvents
    {
        private readonly ILogger<CustomEntityChangeEvents> _logger;

        public CustomEntityChangeEvents(ILogger<CustomEntityChangeEvents> logger)
        {
            _logger = logger;
        }

        public Task OnAdd(EntityAddEventContext context)
        {

            return Task.CompletedTask;
        }

        public Task OnUpdate(EntityUpdateEventContext context)
        {
            return Task.CompletedTask;
        }

        public Task OnDelete(EntityDeleteEventContext context)
        {
            return Task.CompletedTask;
        }
    }
}
