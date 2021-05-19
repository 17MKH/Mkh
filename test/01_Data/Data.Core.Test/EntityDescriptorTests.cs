using Xunit;

namespace Data.Core.Test
{
    public class EntityDescriptorTests : DbContextTests
    {
        //表名称测试
        [Fact]
        public void TableNameTest()
        {
            Assert.Equal("Article", _articleRepository.EntityDescriptor.TableName);
            Assert.Equal("MyCategory", _categoryRepository.EntityDescriptor.TableName);
        }
    }
}
