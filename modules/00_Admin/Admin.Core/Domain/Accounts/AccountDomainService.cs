using System;
using System.Threading.Tasks;
using Mkh.Config.Abstractions;
using Mkh.Domain.Core;
using Mkh.Mod.Admin.Core.Application.Accounts.Dto;
using Mkh.Mod.Admin.Core.Infrastructure;

namespace Mkh.Mod.Admin.Core.Domain.Accounts;

/// <summary>
/// 账户领域服务
/// </summary>
internal class AccountDomainService : BaseDomainService
{
    private readonly IAccountRepository _repository;
    private readonly IPasswordEncryptor _passwordEncryptor;
    private readonly IConfigProvider _configProvider;

    public AccountDomainService(IAccountRepository repository, IPasswordEncryptor passwordEncryptor, IConfigProvider configProvider)
    {
        _repository = repository;
        _passwordEncryptor = passwordEncryptor;
        _configProvider = configProvider;
    }

    /// <summary>
    /// 创建账户
    /// </summary>
    /// <param name="tenantId"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<Account> Create(Guid? tenantId, AccountAddDto dto)
    {
        if (await _repository.ExistsAsync(m => m.Username == dto.Username))
            AdminException.Throw(AdminErrorCode.AccountUsernameExists);

        if (dto.Phone.NotNull() && await _repository.ExistsAsync(m => m.Phone == dto.Phone))
            AdminException.Throw(AdminErrorCode.AccountPhoneExists);

        if (dto.Email.NotNull() && await _repository.ExistsAsync(m => m.Email == dto.Email))
            AdminException.Throw(AdminErrorCode.AccountEmailExists);

        var password = dto.Password;
        if (password.IsNull())
        {
            var config = _configProvider.Get<AdminConfig>();
            password = config.DefaultPassword;
        }

        password = _passwordEncryptor.Encrypt(password);

        var account = new Account(dto.Username, password);

        await _repository.InsertAsync(account);

        return account;
    }

    /// <summary>
    /// 获取账户信息
    /// </summary>
    /// <param name="id">账户编号</param>
    /// <param name="includeRoleIds">包含关联角色信息</param>
    /// <returns></returns>
    public async Task<Account> Get(Guid id, bool includeRoleIds = false)
    {
        var account = await _repository.GetAsync(id);

        if (includeRoleIds)
        {
            account.RoleIds = await _repository.QueryRoleIds(id);
        }

        return account;
    }
}