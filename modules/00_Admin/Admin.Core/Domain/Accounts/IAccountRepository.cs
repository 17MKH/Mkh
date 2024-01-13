using System;
using System.Threading.Tasks;
using Mkh.Domain.Abstractions.Repositories;
using Mkh.Domain.Abstractions.Repositories.Query;
using Mkh.Mod.Admin.Core.Application.Accounts.Dto;

namespace Mkh.Mod.Admin.Core.Domain.Accounts;

/// <summary>
/// 账户仓储接口
/// </summary>
internal interface IAccountRepository : IRepository<Account>
{
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<PagingQueryResult<Account>> QueryAsync(AccountQueryDto dto);

    /// <summary>
    /// 绑定角色
    /// </summary>
    /// <param name="roleIds"></param>
    /// <returns></returns>
    Task BindRolesAsync(Guid[] roleIds);
}