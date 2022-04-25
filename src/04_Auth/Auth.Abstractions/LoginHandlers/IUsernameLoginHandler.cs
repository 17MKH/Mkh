using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Mkh.Auth.Abstractions.LoginHandlers;

/// <summary>
/// 用户名登录处理器
/// </summary>
public interface IUsernameLoginHandler
{
    /// <summary>
    /// 登录处理
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IResultModel<UsernameLoginResult>> Handle(UsernameLoginModel model);
}

/// <summary>
/// 用户名登录
/// </summary>
public class UsernameLoginModel : LoginBaseModel
{
    /// <summary>
    /// 登录方式
    /// </summary>
    public override LoginMode Mode => LoginMode.Username;

    /// <summary>
    /// 用户名
    /// </summary>
    [Required(ErrorMessage = "Please enter your username")]
    public string Username { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    [Required(ErrorMessage = "Please enter your password")]
    public string Password { get; set; }

    /// <summary>
    /// 验证码
    /// </summary>
    public string VerifyCode { get; set; }

    /// <summary>
    /// 验证码ID
    /// </summary>
    public string VerifyCodeId { get; set; }
}

/// <summary>
/// 用户名登录结果
/// </summary>
public class UsernameLoginResult : LoginBaseResult
{

}