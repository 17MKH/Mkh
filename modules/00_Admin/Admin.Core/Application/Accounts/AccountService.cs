using System;
using System.Collections.Generic;
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

    public async Task<PagingQueryResult<AccountDetailsRto>> QueryAsync(AccountQueryDto dto)
    {
        var result = await _repository.QueryAsync(dto);
        var rows = _mapper.Map<IEnumerable<AccountDetailsRto>>(result.Rows);
        return new PagingQueryResult<AccountDetailsRto>(rows, result.Total);
    }

    public async Task<Result<Guid>> CreateAsync(AccountCreateDto dto)
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

        await _repository.InsertAsync(account);

        if (dto.RoleIds.NotNullAndEmpty())
        {
            await _repository.BindRolesAsync(dto.RoleIds);
        }

        return result.Success(account.Id);
    }

    public Task<Result<AccountDetailsRto>> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateAsync(AccountUpdateDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}