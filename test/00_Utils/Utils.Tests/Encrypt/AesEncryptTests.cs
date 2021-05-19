using Microsoft.Extensions.DependencyInjection;
using Mkh.Utils.Encrypt;
using Xunit;

namespace Utils.Tests.Encrypt
{
    public class AesEncryptTests : BaseTest
    {
        private readonly AesEncrypt _desEncrypt;

        public AesEncryptTests()
        {
            _desEncrypt = ServiceProvider.GetService<AesEncrypt>();
        }

        [Fact]
        public void EncryptTest()
        {
            var str = _desEncrypt.Encrypt("17mkh");

            Assert.Equal("3M53b3lHdGeObtJaaroGhQ==", str);
        }

        [Fact]
        public void Encrypt4HexTest()
        {
            var str = _desEncrypt.Encrypt4Hex("17mkh", lowerCase: true);

            Assert.Equal("dcce776f794774678e6ed25a6aba0685", str);
        }


        [Fact]
        public void DecryptTest()
        {
            var str = _desEncrypt.Decrypt("3M53b3lHdGeObtJaaroGhQ==");

            Assert.Equal("17mkh", str);
        }
    }
}
