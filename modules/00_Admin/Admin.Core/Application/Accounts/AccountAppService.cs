using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mkh.Domain.Abstractions.Repositories.Query;
using Mkh.Domain.Core;
using Mkh.Mod.Admin.Core.Application.Accounts.Dto;
using Mkh.Mod.Admin.Core.Application.Accounts.Rto;
using Mkh.Mod.Admin.Core.Domain.Accounts;
using Mkh.Utils.Map;

namespace Mkh.Mod.Admin.Core.Application.Accounts;

internal class AccountAppService : BaseAppService, IAccountAppService
{
    private readonly IMapper _mapper;
    private readonly AccountDomainService _accountDomainService;
    private readonly IAccountRepository _repository;

    public AccountAppService(IMapper mapper, IAccountRepository repository, AccountDomainService accountDomainService)
    {
        _mapper = mapper;
        _repository = repository;
        _accountDomainService = accountDomainService;
    }

    public async Task<PagingQueryResult<AccountInfoRto>> Query(AccountQueryDto dto)
    {
        var result = await _repository.PagingQuery(dto);
        var rows = _mapper.Map<IEnumerable<AccountInfoRto>>(result.Rows);
        return new PagingQueryResult<AccountInfoRto>(rows, result.Total);
    }

    public async Task<Guid> Add(AccountAddDto dto)
    {
        if (dto.RoleIds.NotNullAndEmpty())
        {

        }

        var account = await _accountDomainService.Create(null, dto);
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