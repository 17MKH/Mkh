using System.Threading.Tasks;

namespace Mkh.Mod.Admin.Core.Infrastructure;

/// <summary>
/// 验证码提供器
/// </summary>
public interface IVerifyCodeProvider
{
    /// <summary>
    /// 创建验证码
    /// </summary>
    /// <returns></returns>
    Task<VerifyCodeModel> Create();

    /// <summary>
    /// 校验验证码
    /// </summary>
    /// <param name="id">编号</param>
    /// <param name="code">验证码</param>
    /// <returns></returns>
    Task<IResultModel> Verify(string id, string code);
}

/// <summary>
/// 验证码模型
/// </summary>
public class VerifyCodeModel
{
    public string Id { get; set; }

    public string Base64 { get; set; }
}