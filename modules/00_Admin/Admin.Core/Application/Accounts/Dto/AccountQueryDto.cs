using Mkh.Domain.Abstractions.Repositories.Query;

namespace Mkh.Mod.Admin.Core.Application.Accounts.Dto;

public class AccountQueryDto : PagingQueryBase
{
    /// <summary>
    /// 用户名
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string? Phone { get; set; }
}