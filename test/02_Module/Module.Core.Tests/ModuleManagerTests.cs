using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;
using Mkh.Module.Abstractions;
using Mkh.Module.Admin.Infrastructure.PasswordHandler;
using Mkh.Module.Core;
using Mkh.Utils;
using Xunit;

namespace Module.Core.Tests
{
    public class ModuleManagerTests
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IModuleCollection _modules;

        public ModuleManagerTests()
        {
            var env = new HostingEnvironment { EnvironmentName = "Develop" };
            var cfg = new ConfigurationBuilder().Build();

            var services = new ServiceCollection()
                .AddLogging()
                .AddUtilsServices()
                .AddModules(env, cfg)
                .AddModuleServices();

            _serviceProvider = services.BuildServiceProvider();
            services.AddSingleton(_serviceProvider);

            _serviceProvider = services.BuildServiceProvider();
            ;
            _modules = _serviceProvider.GetService<IModuleCollection>();
        }

        /// <summary>
        /// 模块加载测试
        /// </summary>
        [Fact]
        public void LoadTest()
        {
            Assert.Equal(2, _modules.Count);
        }

        /// <summary>
        /// 模块分层程序集加载测试
        /// </summary>
        [Fact]
        public void ModuleLayerTest()
        {
            var layer = _modules.Get(6220).LayerAssemblies;

            Assert.NotNull(layer.Domain);
            Assert.NotNull(layer.Infrastructure);
            Assert.NotNull(layer.Application);
            Assert.NotNull(layer.Web);
            Assert.Null(layer.Job);

            Assert.Equal("Mkh.Module.Admin.Domain", layer.Domain.GetName().Name);
        }

        /// <summary>
        /// 模块服务配置器测试
        /// </summary>
        [Fact]
        public void ModuleServicesConfiguratorTest()
        {
            var passwordHandler = _serviceProvider.GetService<IPasswordHandler>();
            var pwd = passwordHandler.Encrypt("iamoldli", "iamoldli");

            Assert.Equal("E532B7F6B03E0CB5AA074E82A4B5BE7F", pwd);
        }

        /// <summary>
        /// 模块枚举信息测试
        /// </summary>
        [Fact]
        public void ModuleEnumTest()
        {
            var accountTypeOptions = _modules.Get(6220).GetEnum("AccountType").Options;

            Assert.Equal(2, accountTypeOptions.Count);
        }
    }
}
