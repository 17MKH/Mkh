using Microsoft.AspNetCore.Http;

namespace Mkh.Utils.Web.Extensions
{
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// 获取指定路径的完整URL
        /// </summary>
        /// <param name="request"></param>
        /// <param name="path">子路径</param>
        /// <returns></returns>
        public static string GetUrl(this HttpRequest request, string path = null)
        {
            return $"{request.Scheme}://{request.Host}{request.PathBase}{path ?? string.Empty}";
        }
    }
}
