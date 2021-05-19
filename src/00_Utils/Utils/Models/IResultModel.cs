using System.Text.Json.Serialization;

namespace Mkh.Utils.Models
{
    /// <summary>
    /// 返回结果模型接口
    /// </summary>
    public interface IResultModel
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        [JsonIgnore]
        bool Successful { get; }

        /// <summary>
        /// 状态码，1、成功 0、失败
        /// </summary>
        public int Code => Successful ? 1 : 0;

        /// <summary>
        /// 错误信息
        /// </summary>
        string Msg { get; }

        /// <summary>
        /// 业务码，用于业务中自定义
        /// </summary>
        [JsonPropertyName("ucode")]
        string BusinessCode { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        long Timestamp { get; }
    }

    /// <summary>
    /// 返回结果模型泛型接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IResultModel<out T> : IResultModel
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        T Data { get; }
    }
}
