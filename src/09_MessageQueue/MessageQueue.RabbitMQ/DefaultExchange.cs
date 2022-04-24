namespace Mkh.MessageQueue.RabbitMQ;

/// <summary>
/// 默认交换器
/// </summary>
internal static class DefaultExchange
{
    public const string Direct = "nm.direct";

    public const string Fanout = "nm.fanout";

    public const string Topic = "nm.topic";

    public const string Headers = "nm.headers";
}