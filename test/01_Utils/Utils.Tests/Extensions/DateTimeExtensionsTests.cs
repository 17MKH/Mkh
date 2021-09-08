using System;
using Mkh;
using Mkh.Utils.Enums;
using Xunit;

namespace Utils.Tests.Extensions
{
    public class DateTimeExtensionsTests
    {
        private DateTime _date = new DateTime(2020, 7, 6, 21, 14, 0);

        [Fact]
        public void FormatTest()
        {
            var format = _date.Format();
            Assert.Equal("2020-07-06 21:14:00", format);

            format = _date.Format("yyyy-MM-dd");
            Assert.Equal("2020-07-06", format);
        }

        [Fact]
        public void ToTimestampTest()
        {
            var timestamp = _date.ToTimestamp();

            Assert.Equal(1594041240, timestamp);
        }

        [Fact]
        public void GetWeekTest()
        {
            var week = _date.GetWeek();
            Assert.Equal(Week.Monday, week);
        }
    }
}
