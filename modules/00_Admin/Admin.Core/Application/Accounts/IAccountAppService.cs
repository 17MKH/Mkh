using System;
using System.Threading.Tasks;
using Mkh.Domain.Abstractions;
using Mkh.Domain.Abstractions.Repositories.Query;
using Mkh.Mod.Admin.Core.Application.Accounts.Dto;
using Mkh.Mod.Admin.Core.Application.Accounts.Rto;

namespace Mkh.Mod.Admin.Core.Application.Accounts;

public interface IAccountAppService : IAppService
{
    /// <summary>
    /// 查询
    /// </summary>
    /// <returns></returns>
    Task<PagingQueryResult<AccountInfoRto>> Query(AccountQueryDto dto);

    /// <summary>
    /// 添加账户，返回主键
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<Guid> Add(AccountAddDto dto);

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<AccountInfoRto> Edit(Guid id);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task Update(AccountUpdateDto dto);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task Delete(Guid id);

    /// <summary>
    /// 更新皮肤配置
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task UpdateSkin(AccountSkinUpdateDto dto);

    /// <summary>
    /// 激活账户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task Activate(Guid id);
}