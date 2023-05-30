using Mkh;
using Xunit;

namespace Utils.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Fact]
        public void FirstCharToLowerTest()
        {
            var str = "Name";
            str = str.FirstCharToLower();

            Assert.Equal("name", str);

            str = "NAME";
            str = str.FirstCharToLower();
            Assert.Equal("nAME", str);
        }

        [Fact]
        public void FirstCharToUpperTest()
        {
            var str = "name";
            str = str.FirstCharToUpper();

            Assert.Equal("Name", str);
        }

        [Fact]
        public void ToBase64Test()
        {
            var str = "iamoldli".ToBase64();

            Assert.Equal("aWFtb2xkbGk=", str);
        }
    }
}
