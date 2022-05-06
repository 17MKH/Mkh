using Microsoft.Extensions.DependencyInjection;
using Mkh.Utils.Annotations;
using Xunit;

namespace Utils.Tests.Extensions
{
    public class ServiceProviderExtensionsTests : BaseTest
    {
        [Fact]
        public void GetStartWithTest()
        {
            var test = ServiceProvider.GetStartWith<ITest>("oldli");
            Assert.Equal("OLDLI", test.Test());
        }
    }

    public interface ITest
    {
        string Test();
    }

    [SingletonInject]
    public class OldliTest : ITest
    {
        public string Test()
        {
            return "OLDLI";
        }
    }

    [SingletonInject]
    public class LaoliTest : ITest
    {
        public string Test()
        {
            return "LAOLI";
        }
    }
}
