using System;
using System.Threading.Tasks;
using Mkh.Domain.Abstractions;
using Mkh.Domain.Abstractions.Repositories.Query;
using Mkh.Mod.Admin.Core.Application.Accounts.Dto;
using Mkh.Mod.Admin.Core.Application.Accounts.Rto;

namespace Mkh.Mod.Admin.Core.Application.Accounts;

public interface IAccountService : IAppService
{
    /// <summary>
    /// 查询
    /// </summary>
    /// <returns></returns>
    Task<PagingQueryResult<AccountDetailsRto>> QueryAsync(AccountQueryDto dto);

    /// <summary>
    /// 创建账户，返回主键
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<Result<Guid>> CreateAsync(AccountCreateDto dto);

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Result<AccountDetailsRto>> GetAsync(Guid id);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<Result> UpdateAsync(AccountUpdateDto dto);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Result> DeleteAsync(Guid id);
}