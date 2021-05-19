using Microsoft.Extensions.DependencyInjection;
using Mkh.Utils.Encrypt;
using Xunit;

namespace Utils.Tests.Encrypt
{
    public class TripleDesEncryptTests : BaseTest
    {
        private readonly TripleDesEncrypt _desEncrypt;

        public TripleDesEncryptTests()
        {
            _desEncrypt = ServiceProvider.GetService<TripleDesEncrypt>();
        }

        [Fact]
        public void EncryptTest()
        {
            var str = _desEncrypt.Encrypt("17mkh");

            Assert.Equal("gxo1ddAu6qQ=", str);
        }

        [Fact]
        public void Encrypt4HexTest()
        {
            var str = _desEncrypt.Encrypt4Hex("17mkh", lowerCase: true);

            Assert.Equal("831a3575d02eeaa4", str);
        }
    }
}
