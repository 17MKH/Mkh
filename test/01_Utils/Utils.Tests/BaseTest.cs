using System;
using Microsoft.Extensions.DependencyInjection;
using Mkh.Utils;

namespace Utils.Tests
{
    public class BaseTest
    {
        protected readonly IServiceProvider ServiceProvider;

        public BaseTest()
        {
            var services = new ServiceCollection();
            services.AddServicesFromAttribute();
            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
