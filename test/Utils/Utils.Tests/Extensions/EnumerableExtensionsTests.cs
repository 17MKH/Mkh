using System.Linq;
using DistinctBy = Mkh;
using Xunit;

namespace Utils.Tests.Extensions
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void DistinctByTest()
        {
            var arr = new string[5];
            arr[0] = "1";
            arr[1] = "2";
            arr[2] = "2";
            arr[3] = "3";
            arr[4] = "4";
            var m = arr.DistinctBy(m => m);

            Assert.Equal(4, m.Count());
            Assert.Equal(1, m.Count(m => m == "2"));
        }
    }
}
