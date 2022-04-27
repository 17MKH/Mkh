using Xunit;
using Xunit.Abstractions;

namespace Data.Adapter.Sqlite.Test
{
    public class EntityChangeEventsTests : BaseTest
    {
        public EntityChangeEventsTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Test()
        {
            Assert.Equal(1, _dbContext.EntityChangeEvents.Count);
        }
    }
}
