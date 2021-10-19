using System;
using Mkh.Utils.Enums;
using Mkh.Utils.Helpers;

// ReSharper disable once CheckNamespace
namespace Mkh;

public static class DateTimeExtensions
{
    /// <summary>
    /// 日期格式化
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="format">默认：yyyy-MM-dd HH:mm:ss</param>
    /// <returns></returns>
    public static string Format(this DateTime dateTime, string format = null)
    {
        if (format.IsNull())
            format = "yyyy-MM-dd HH:mm:ss";

        return dateTime.ToString(format);
    }

    /// <summary>
    /// 转换为时间戳
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="milliseconds">是否使用毫秒</param>
    /// <returns></returns>
    public static long ToTimestamp(this DateTime dateTime, bool milliseconds = false)
    {
        var ts = dateTime.ToUniversalTime() - DateTimeHelper.TimestampStart;
        return (long)(milliseconds ? ts.TotalMilliseconds : ts.TotalSeconds);
    }

    /// <summary>
    /// 获取周几
    /// </summary>
    /// <param name="datetime"></param>
    /// <returns></returns>
    public static Week GetWeek(this DateTime datetime)
    {
        var dayOfWeek = datetime.DayOfWeek.ToInt();
        switch (dayOfWeek)
        {
            case 0:
                return Week.Sunday;
            case 1:
                return Week.Monday;
            case 2:
                return Week.Tuesday;
            case 3:
                return Week.Wednesday;
            case 4:
                return Week.Thursday;
            case 5:
                return Week.Friday;
            default:
                return Week.Saturday;
        }
    }
}