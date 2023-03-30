using Mkh.Utils.Json.Converters;

namespace Mkh.Auth.Abstractions
{
    /// <summary>
    /// 凭据
    /// </summary>
    [JsonPolymorphism]
    public interface ICredential
    {
    }
}
