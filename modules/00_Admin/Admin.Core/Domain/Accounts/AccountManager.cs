using Mkh.Auth.Abstractions;
using Mkh.Mod.Admin.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mkh.Mod.Admin.Core.Domain.Accounts;

internal class AccountManager
{
    private readonly IAccountRepository _repository;
    private readonly IPasswordEncryptor _passwordEncryptor;
    private readonly ITenantResolver _tenantResolver;

    public AccountManager(IAccountRepository repository, IPasswordEncryptor passwordEncryptor, ITenantResolver tenantResolver)
    {
        _repository = repository;
        _passwordEncryptor = passwordEncryptor;
        _tenantResolver = tenantResolver;
    }

    /// <summary>
    /// 创建账户
    /// </summary>
    /// <param name="username"></param>
    /// <param name="phone"></param>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <param name="roleIds"></param>
    /// <returns></returns>
    public async Task<Result<Guid>> Create(string username, string phone, string email, string password, IEnumerable<Guid> roleIds)
    {
        var result = ResultBuilder.Success<Guid>();

        if (await _repository.ExistsAsync(m => m.Username == username))
            return result.Failed(AdminErrorCode.AccountUsernameExists);

        if (phone.NotNull() && await _repository.ExistsAsync(m => m.Phone == phone))
            result.Failed(AdminErrorCode.AccountPhoneExists);

        if (email.NotNull() && await _repository.ExistsAsync(m => m.Email == email))
            result.Failed(AdminErrorCode.AccountEmailExists);

        password = _passwordEncryptor.Encrypt(password);

        var tenantId = await _tenantResolver.ResolveId();

        var account = new Account(tenantId, username, password)
        {
            Phone = phone,
            Email = email,
            RoleIds = roleIds
        };

        await _repository.InsertAsync(account);

        return result.Success(account.Id);
    }
}