using System.Text.Json;
using Mkh.Utils;
using Xunit;

namespace Utils.Tests.Models
{
    public class ResultModelTests
    {
        [Fact]
        public void SuccessTest()
        {
            var result = Result.Success();
            var json = JsonSerializer.Serialize(result);

            Assert.NotNull(json);
        }
    }
}
