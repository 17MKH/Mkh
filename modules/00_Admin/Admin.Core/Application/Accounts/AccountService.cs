using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    private readonly IRoleService _roleService;
    private readonly AccountManager _manager;

    public AccountService(IMapper mapper, IRoleService roleService, AccountManager manager)
    {
        _mapper = mapper;
        _roleService = roleService;
        _manager = manager;
    }

    public async Task<PagingQueryResult<AccountDetailsRto>> QueryAsync(AccountQueryDto dto)
    {
        var result = await _repository.QueryAsync(dto);
        var rows = _mapper.Map<IEnumerable<AccountDetailsRto>>(result.Rows);
        return new PagingQueryResult<AccountDetailsRto>(rows, result.Total);
    }

    public async Task<Result<Guid>> CreateAsync(AccountCreateDto dto)
    {
        if (!await _roleService.IsExistAsync(dto.RoleIds))
        {
            ResultBuilder.Failed<Guid>(AdminErrorCode.AccountBindRoleExists);
        }

        return await _manager.Create(dto.Username, dto.Name, dto.Email, dto.Password, dto.RoleIds);
    }

    public Task<Result<AccountDetailsRto>> GetAsync(Guid id)
    {
        
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