using System;
using Mkh;
using Mkh.Utils.Enums;
using Xunit;

namespace Utils.Tests.Extensions
{
    public class CommonExtensionsTests
    {
        [Theory]
        [InlineData("1")]
        [InlineData("a")]
        public void ToByteTest(string val)
        {
            var b = val.ToByte();

            Assert.True(b >= 0);
        }

        [Theory]
        [InlineData("A")]
        public void ToCharTest(string val)
        {
            var c = val.ToChar();

            Assert.Equal('A', c);
        }

        [Theory]
        [InlineData(65)]
        public void ToCharForIntTest(int val)
        {
            var c = val.ToChar();

            Assert.Equal('A', c);
        }

        [Theory]
        [InlineData("10")]
        public void ToShortTest(string val)
        {
            var c = val.ToShort();

            Assert.Equal(10, c);
        }

        [Theory]
        [InlineData(Sex.Girl)]
        [InlineData("10")]
        public void ToIntTest(object val)
        {
            var c = val.ToInt();

            Assert.True(c > 0);
        }

        [Theory]
        [InlineData("123432432")]
        public void ToLongTest(string val)
        {
            var c = val.ToLong();

            Assert.Equal(123432432, c);
        }

        [Theory]
        [InlineData("1234324.32")]
        public void ToFloatTest(string val)
        {
            var c = val.ToFloat();

            Assert.Equal(1234324.32f, c);
        }

        [Theory]
        [InlineData("1234324.32")]
        public void ToDoubleTest(string val)
        {
            var c = val.ToDouble();

            Assert.Equal(1234324.32d, c);
        }

        [Theory]
        [InlineData("1234324.32")]
        public void ToDecimalTest(string val)
        {
            var c = val.ToDecimal();

            Assert.Equal(1234324.32m, c);
        }

        [Theory]
        [InlineData("2020-07-05 16:23")]
        public void ToDateTimeTest(string val)
        {
            var c = val.ToDateTime();

            Assert.Equal(new DateTime(2020, 7, 5, 16, 23, 00), c);
        }

        [Theory]
        [InlineData("2020-07-05 16:23")]
        public void ToDateTest(string val)
        {
            var c = val.ToDate();

            Assert.Equal(new DateTime(2020, 7, 5), c);
        }

        [Fact]
        public void ToBoolTest()
        {
            var c = "1".ToBool();
            var c1 = 0.ToBool();
            var c2 = "是".ToBool();
            var c3 = "否".ToBool();
            var c4 = "yes".ToBool();
            var c5 = "no".ToBool();

            Assert.True(c);
            Assert.False(c1);
            Assert.True(c2);
            Assert.False(c3);
            Assert.True(c4);
            Assert.False(c5);
        }

        [Fact]
        public void ToGuidTest()
        {
            var guid = Guid.NewGuid();

            guid = guid.ToString().ToGuid();

            Assert.NotEqual(Guid.Empty, guid);
        }

        [Fact]
        public void ToTest()
        {
            var v = "1".To<int>();

            Assert.True(v > 0);

            var v1 = "2020-07-05".To<DateTime>();

            Assert.Equal(new DateTime(2020, 7, 5), v1);

            var v2 = 1.To<bool>();

            Assert.True(v2);
        }

        [Fact]
        public void BoolToTest()
        {
            var b = true;
            var v = b.ToIntString();

            Assert.Equal("1", v);

            var v1 = b.ToInt();

            Assert.Equal(1, v1);

            var v2 = b.ToZhCn();

            Assert.Equal("是", v2);
        }

        [Fact]
        public void ToHexTest()
        {
            var str = "iamoldli".ToHex();
            Assert.Equal("69616d6f6c646c69", str);
        }


        [Fact]
        public void Hex2StringTest()
        {
            var str = "69616d6f6c646c69313233".Hex2String();
            Assert.Equal("iamoldli123", str);
        }
    }
}
