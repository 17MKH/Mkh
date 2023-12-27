using System;
using System.Collections.Generic;
using System.Threading;
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
    Task<PagingQueryResult<Account>> PagingQuery(AccountQueryDto dto);

    /// <summary>
    /// 根据用户名查询账户信息
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<Account> GetByUserName(string username, CancellationToken cancellationToken = default);

    /// <summary>
    /// 查询指定账户关联的角色编号列表
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<List<Guid>> QueryRoleIds(Guid id);
}