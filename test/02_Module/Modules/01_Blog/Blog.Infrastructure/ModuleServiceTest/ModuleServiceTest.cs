using Microsoft.Extensions.Logging;

namespace Mkh.Module.Blog.Infrastructure.ModuleServiceTest
{
    public class ModuleServiceTest : IModuleServiceTest
    {
        private readonly ILogger<ModuleServiceTest> _logger;

        public ModuleServiceTest(ILogger<ModuleServiceTest> logger)
        {
            _logger = logger;
        }

        public void Test()
        {
            _logger.LogDebug("测试服务");
        }
    }
}
