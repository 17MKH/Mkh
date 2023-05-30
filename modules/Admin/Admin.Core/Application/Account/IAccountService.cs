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
    /// 添加
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<IResultModel> Add(AccountAddDto dto);

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IResultModel> Edit(Guid id);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<IResultModel> Update(AccountUpdateDto dto);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IResultModel> Delete(Guid id);

    /// <summary>
    /// 更新皮肤配置
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<IResultModel> UpdateSkin(AccountSkinUpdateDto dto);

    /// <summary>
    /// 激活账户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> Activate(Guid id);
}