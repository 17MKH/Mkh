﻿using System;
using Mkh.Utils.Annotations;

namespace Mkh.Utils.Helpers;

[SingletonInject]
public class DateTimeHelper
{
    /// <summary>
    /// 时间戳起始日期
    /// </summary>
    public static readonly DateTime TimestampStart = new DateTime(1970, 1, 1, 0, 0, 0, 0);

    /// <summary>
    /// 获取时间戳
    /// </summary>
    /// <param name="milliseconds">是否使用毫秒</param>
    /// <returns></returns>
    public string GetTimestamp(bool milliseconds = false)
    {
        var ts = DateTime.UtcNow - TimestampStart;
        return Convert.ToInt64(milliseconds ? ts.TotalMilliseconds : ts.TotalSeconds).ToString();
    }

    /// <summary>
    /// 时间戳转日期
    /// </summary>
    /// <param name="timestamp">时间戳</param>
    /// <param name="milliseconds">是否使用毫秒</param>
    /// <returns></returns>
    public DateTime Timestamp2DateTime(long timestamp, bool milliseconds = false)
    {
        var val = milliseconds ? 10000 : 10000000;
        return TimestampStart.AddTicks(timestamp * val);
    }

    /// <summary>判断当前年份是否是闰年</summary>
    /// <param name="year">年份</param>
    /// <returns></returns>
    private bool IsLeapYear(int year)
    {
        int n = year;
        if ((n % 400 == 0) || (n % 4 == 0 && n % 100 != 0))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 获取周几
    /// </summary>
    /// <returns></returns>
    public string GetWeek()
    {
        var dayOfWeek = DateTime.Now.DayOfWeek.ToInt();
        string week;
        switch (dayOfWeek)
        {
            case 0:
                week = "星期日";
                break;
            case 1:
                week = "星期一";
                break;
            case 2:
                week = "星期二";
                break;
            case 3:
                week = "星期三";
                break;
            case 4:
                week = "星期四";
                break;
            case 5:
                week = "星期五";
                break;
            default:
                week = "星期六";
                break;
        }
        return week;
    }
}