using Microsoft.Extensions.DependencyInjection;
using Mkh.Utils.Encrypt;
using Xunit;

namespace Utils.Tests.Encrypt
{
    public class Md5EncryptTests : BaseTest
    {
        private readonly Md5Encrypt _encrypt;

        public Md5EncryptTests()
        {
            _encrypt = ServiceProvider.GetService<Md5Encrypt>();
        }

        [Fact]
        public void EncryptTest()
        {
            var str = _encrypt.Encrypt("iamoldli");

            Assert.Equal("5108C4039CD470FD89B626E03EC0ABEE", str);
        }
    }
}
