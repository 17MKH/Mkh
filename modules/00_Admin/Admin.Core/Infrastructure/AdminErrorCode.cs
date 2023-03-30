namespace Mkh.Mod.Admin.Core.Infrastructure;

public enum AdminErrorCode
{
    /// <summary>
    /// 用户名已存在
    /// </summary>
    AccountUsernameExists,
    /// <summary>
    /// 手机号已存在
    /// </summary>
    AccountPhoneExists,
    /// <summary>
    /// 邮箱已存在
    /// </summary>
    AccountEmailExists,
    /// <summary>
    /// 账户绑定角色不存在
    /// </summary>
    AccountBindRoleExists,

    /// <summary>
    /// 用户名登录失败
    /// </summary>
    AuthorizeUsernameLoginFailed,
    /// <summary>
    /// 刷新令牌无效
    /// </summary>
    AuthorizeRefreshTokenInvalid,
    /// <summary>
    /// 账户不存在
    /// </summary>
    AuthorizeAccountNotExists,
    /// <summary>
    /// 账户已禁用
    /// </summary>
    AuthorizeAccountDisabled,

    /// <summary>
    /// 字典不存在
    /// </summary>
    DictNotExists,
    /// <summary>
    /// 字典编码已存在
    /// </summary>
    DictCodeExists,
    /// <summary>
    /// 字典包含数据项
    /// </summary>
    DictIncludeDataItem,
    /// <summary>
    /// 字典分组不存在
    /// </summary>
    DictGroupNotExists,
    /// <summary>
    /// 字典分组编码已存在
    /// </summary>
    DictGroupCodeExists,
    /// <summary>
    /// 字典分组包含字典数据
    /// </summary>
    DictGroupIncludeDict,
    /// <summary>
    /// 字典项值已存在
    /// </summary>
    DictItemValueExists,

    /// <summary>
    /// 模块编码为空
    /// </summary>
    MenuModuleCodeNull,
    /// <summary>
    /// 路由名称为空
    /// </summary>
    MenuRouteNameNull,
    /// <summary>
    /// 连接地址为空
    /// </summary>
    MenuUrlNull,
    /// <summary>
    /// 自定义Js为空
    /// </summary>
    MenuCustomJsNull,
    /// <summary>
    /// 父级菜单不存在
    /// </summary>
    MenuParentNotExists,
    /// <summary>
    /// 菜单分组不允许删除
    /// </summary>
    MenuGroupNotAllowDelete,

    /// <summary>
    /// 角色名称已存在
    /// </summary>
    RoleNameExists,
    /// <summary>
    /// 角色编码已存在
    /// </summary>
    RoleCodeExists
}