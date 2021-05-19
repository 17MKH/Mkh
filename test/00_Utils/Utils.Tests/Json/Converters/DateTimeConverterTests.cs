using System;
using System.Text.Json;
using Mkh.Utils.Json.Converters;
using Xunit;

namespace Utils.Tests.Json.Converters
{
    /// <summary>
    /// JSON 日期转换器测试
    /// </summary>
    public class DateTimeConverterTests : BaseTest
    {
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions();

        public DateTimeConverterTests()
        {
            _options.Converters.Add(new DateTimeConverter());
        }

        [Fact]
        public void SerializeTest()
        {
            var p = new Test
            {
                Time = new DateTime(2020, 9, 22, 10, 51, 00)
            };

            var str = JsonSerializer.Serialize(p, _options);

            Assert.Equal("{\"Time\":\"2020-09-22 10:51:00\"}", str);
        }

        [Fact]
        public void DeserializeTest()
        {
            var str = "{\"Time\":\"2020-09-22 10:51:00\"}";

            var t = JsonSerializer.Deserialize<Test>(str, _options);

            Assert.Equal(new DateTime(2020, 9, 22, 10, 51, 00), t.Time);
        }
    }

    public class Test
    {
        public DateTime Time { get; set; }
    }
}
