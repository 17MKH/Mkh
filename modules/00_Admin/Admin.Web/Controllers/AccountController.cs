using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Mkh.Auth.Abstractions.Annotations;
using Mkh.Config.Abstractions;
using Mkh.Data.Abstractions.Query;
using Mkh.Identity.Abstractions;
using Mkh.Mod.Admin.Core.Domain.Account;
using Mkh.Mod.Admin.Core.Infrastructure;

namespace Mkh.Mod.Admin.Web.Controllers;

[Tags("账户管理")]
public class AccountController : Web.ModuleController
{
    private readonly IAccountService _service;
    private readonly ICurrentAccount _account;
    private readonly IConfigProvider _configProvider;

    public AccountController(IAccountService service, ICurrentAccount account, IConfigProvider configProvider)
    {
        _service = service;
        _account = account;
        _configProvider = configProvider;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <remarks>查询角色列表</remarks>
    [HttpGet]
    public Task<PagingQueryResultModel<AccountEntity>> Query([FromQuery] AccountQueryDto dto)
    {
        return _service.Query(dto);
    }

    /// <summary>
    /// 添加账户，添加成功返回新增账户ID
    /// </summary>
    /// <remarks></remarks>
    [HttpPost]
    public Task<IResultModel<Guid>> Add(AccountAddDto dto)
    {
        return Success(_service.Add(dto));
    }

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<IResultModel<AccountUpdateDto>> Edit(Guid id)
    {
        return Success(_service.Edit(id));
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <remarks></remarks>
    [HttpPost]
    public Task<IResultModel> Update(AccountUpdateDto dto)
    {
        return Success(_service.Update(dto));
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public Task<IResultModel> Delete([BindRequired] Guid id)
    {
        return Success(_service.Delete(id));
    }

    /// <summary>
    /// 获取账户默认密码
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IResultModel DefaultPassword()
    {
        var config = _configProvider.Get<AdminConfig>();
        return ResultModel.Success(config.DefaultPassword);
    }

    /// <summary>
    /// 更新账户皮肤配置
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [AllowWhenAuthenticated]
    [HttpPost]
    public Task<IResultModel> UpdateSkin(AccountSkinUpdateDto dto)
    {
        dto.AccountId = _account.Id;

        return Success(_service.UpdateSkin(dto));
    }
}