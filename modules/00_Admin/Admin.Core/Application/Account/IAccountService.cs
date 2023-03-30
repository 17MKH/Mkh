using System;
using System.Threading.Tasks;
using Mkh.Data.Abstractions.Query;
using Mkh.Mod.Admin.Core.Application.Account.Dto;
using Mkh.Mod.Admin.Core.Domain.Account;

namespace Mkh.Mod.Admin.Core.Application.Account;

public interface IAccountService
{
    /// <summary>
    /// 查询
    /// </summary>
    /// <returns></returns>
    Task<PagingQueryResultModel<AccountEntity>> Query(AccountQueryDto dto);

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
    Task<AccountUpdateDto> Edit(Guid id);

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