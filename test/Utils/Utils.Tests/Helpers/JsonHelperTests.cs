using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Mkh;
using Mkh.Utils.Helpers;
using Mkh.Utils.Json;
using Xunit;

namespace Utils.Tests.Helpers
{
    public class JsonHelperTests : BaseTest
    {
        private readonly JsonHelper _helper;

        public JsonHelperTests()
        {
            _helper = ServiceProvider.GetService<JsonHelper>();
        }

        [Fact]
        public void SerializeTest()
        {
            var p = new Product
            {
                Id = 1,
                Title = "test",
                Price = 3.2M,
                Tags = new List<string> { "test" },
                PublishDate = "2020-09-22 13:57".ToDateTime()
            };

            var json = _helper.Serialize(p);

            Assert.Equal("{\"id\":1,\"title\":\"test\",\"price\":3.2,\"tags\":[\"test\"],\"publishDate\":\"2020-09-22 13:57:00\"}", json);
        }

        [Fact]
        public void DeserializeTest()
        {
            var json =
                "{\"id\":1,\"title\":\"test\",\"price\":3.2,\"tags\":[\"test\"],\"publishDate\":\"2020-09-22 13:57\"}";

            var p = _helper.Deserialize<Product>(json);

            Assert.Equal(3.2M, p.Price);
        }
    }
}
