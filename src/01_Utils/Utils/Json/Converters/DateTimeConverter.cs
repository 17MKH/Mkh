using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mkh.Utils.Json.Converters;

/// <summary>
/// Json日期类型转换器
/// </summary>
public class DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var json = reader.GetString();
        if (json.IsNull())
            return DateTime.MinValue;

        return json.ToDateTime();
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
    }
}