using Mkh;
using Xunit;

namespace Utils.Tests.Extensions
{
    public class ArrayExtensionsTests
    {
        [Fact]
        public void RandomGetTest()
        {
            var arr = new string[5];
            arr[0] = "1";
            arr[1] = "2";
            arr[2] = "3";
            arr[3] = "4";
            arr[4] = "5";

            var item = arr.RandomGet();

            Assert.NotNull(item);
        }
    }
}
