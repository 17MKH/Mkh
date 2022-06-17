using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Mkh.Auth.Abstractions;
using Mkh.Auth.Abstractions.Annotations;
using Mkh.Config.Abstractions;
using Mkh.Data.Abstractions.Query;
using Mkh.Mod.Admin.Core.Application.Account;
using Mkh.Mod.Admin.Core.Application.Account.Dto;
using Mkh.Mod.Admin.Core.Domain.Account;
using Mkh.Mod.Admin.Core.Infrastructure;
using Swashbuckle.AspNetCore.Annotations;

namespace Mkh.Mod.Admin.Web.Controllers;

[SwaggerTag("账户管理")]
public class AccountController : Web.ModuleController
{
    private readonly IAccountService _service;
    private readonly IAccount _account;
    private readonly IConfigProvider _configProvider;

    public AccountController(IAccountService service, IAccount account, IConfigProvider configProvider)
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
    /// 添加
    /// </summary>
    /// <remarks></remarks>
    [HttpPost]
    public Task<IResultModel> Add(AccountAddDto dto)
    {
        return _service.Add(dto);
    }

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<IResultModel> Edit(Guid id)
    {
        return _service.Edit(id);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <remarks></remarks>
    [HttpPost]
    public Task<IResultModel> Update(AccountUpdateDto dto)
    {
        return _service.Update(dto);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public Task<IResultModel> Delete([BindRequired] Guid id)
    {
        return _service.Delete(id);
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

        return _service.UpdateSkin(dto);
    }
}