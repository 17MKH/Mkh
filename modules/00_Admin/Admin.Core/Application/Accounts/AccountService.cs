using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mkh.Auth.Abstractions;
using Mkh.Domain.Abstractions.Repositories.Query;
using Mkh.Domain.Core;
using Mkh.Mod.Admin.Core.Application.Accounts.Dto;
using Mkh.Mod.Admin.Core.Application.Accounts.Rto;
using Mkh.Mod.Admin.Core.Application.Roles;
using Mkh.Mod.Admin.Core.Domain.Accounts;
using Mkh.Mod.Admin.Core.Infrastructure;
using Mkh.Utils.Map;

namespace Mkh.Mod.Admin.Core.Application.Accounts;

internal class AccountService : BaseAppService, IAccountService
{
    private readonly IMapper _mapper;
    private readonly IAccountRepository _repository;
    private readonly IPasswordEncryptor _passwordEncryptor;
    private readonly ITenantResolver _tenantResolver;
    private readonly IRoleService _roleService;

    public AccountService(IMapper mapper, IAccountRepository repository, IPasswordEncryptor passwordEncryptor, ITenantResolver tenantResolver, IRoleService roleService)
    {
        _mapper = mapper;
        _repository = repository;
        _passwordEncryptor = passwordEncryptor;
        _tenantResolver = tenantResolver;
        _roleService = roleService;
    }

    public async Task<PagingQueryResult<AccountInfoRto>> Query(AccountQueryDto dto)
    {
        var result = await _repository.PagingQuery(dto);
        var rows = _mapper.Map<IEnumerable<AccountInfoRto>>(result.Rows);
        return new PagingQueryResult<AccountInfoRto>(rows, result.Total);
    }

    public async Task<Result<Guid>> Create(AccountCreateDto dto)
    {
        var result = ResultBuilder.Success<Guid>();

        if (await _repository.ExistsAsync(m => m.Username == dto.Username))
            return result.Failed(AdminErrorCode.AccountUsernameExists);

        if (dto.Phone.NotNull() && await _repository.ExistsAsync(m => m.Phone == dto.Phone))
            result.Failed(AdminErrorCode.AccountPhoneExists);

        if (dto.Email.NotNull() && await _repository.ExistsAsync(m => m.Email == dto.Email))
            result.Failed(AdminErrorCode.AccountEmailExists);

        if (!await _roleService.IsExistAsync(dto.RoleIds))
        {
            result.Failed(AdminErrorCode.AccountBindRoleExists);
        }

        var password = _passwordEncryptor.Encrypt(dto.Password);

        var tenantId = await _tenantResolver.ResolveId();

        var account = new Account(tenantId, dto.Username, password)
        {
            Phone = dto.Phone,
            Email = dto.Email,
        };

        if (dto.RoleIds.NotNullAndEmpty())
        {
            account.Roles= dto.RoleIds.Select(m=>new AccountRoleRelation())
        }

        await _repository.InsertAsync(account);

        return result.Success(account.Id);
    }

    public Task<AccountInfoRto> Edit(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task Update(AccountUpdateDto dto)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateSkin(AccountSkinUpdateDto dto)
    {
        throw new NotImplementedException();
    }

    public Task Activate(Guid id)
    {
        throw new NotImplementedException();
    }
}