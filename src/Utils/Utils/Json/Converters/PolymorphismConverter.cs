using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mkh.Utils.Json.Converters
{
    /// <summary>
    /// 多态序列化抽象类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PolymorphismConverter<T> : JsonConverter<T>
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }


    /// <summary>
    /// Json多态序列化特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class JsonPolymorphismAttribute : Attribute
    {

    }
}
