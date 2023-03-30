using System;
using System.Threading.Tasks;
using Mkh.Config.Abstractions;
using Mkh.Data.Abstractions.Query;
using Mkh.Mod.Admin.Core.Application.Account.Dto;
using Mkh.Mod.Admin.Core.Domain.Account;
using Mkh.Mod.Admin.Core.Domain.AccountSkin;
using Mkh.Mod.Admin.Core.Domain.Role;
using Mkh.Mod.Admin.Core.Infrastructure;
using Mkh.Utils.Map;

namespace Mkh.Mod.Admin.Core.Application.Account;

internal class AccountService : IAccountService
{
    private readonly IMapper _mapper;
    private readonly IAccountRepository _repository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHandler _passwordHandler;
    private readonly IAccountSkinRepository _skinRepository;
    private readonly IConfigProvider _configProvider;

    public AccountService(IMapper mapper, IAccountRepository repository, IPasswordHandler passwordHandler, IRoleRepository roleRepository, IAccountSkinRepository skinRepository, IConfigProvider configProvider)
    {
        _mapper = mapper;
        _repository = repository;
        _passwordHandler = passwordHandler;
        _roleRepository = roleRepository;
        _skinRepository = skinRepository;
        _configProvider = configProvider;
    }

    public Task<PagingQueryResultModel<AccountEntity>> Query(AccountQueryDto dto)
    {
        return _repository.Query(dto);
    }

    public async Task<Guid> Add(AccountAddDto dto)
    {
        if (await _repository.ExistsUsername(dto.Username))
            AdminException.Throw(AdminErrorCode.AccountUsernameExists);

        if (dto.Phone.NotNull() && await _repository.ExistsPhone(dto.Phone))
            AdminException.Throw(AdminErrorCode.AccountPhoneExists);

        if (dto.Email.NotNull() && await _repository.ExistsUsername(dto.Email))
            AdminException.Throw(AdminErrorCode.AccountEmailExists);

        if (!await _roleRepository.Exists(dto.RoleId))
            AdminException.Throw(AdminErrorCode.AccountBindRoleExists);

        var account = _mapper.Map<AccountEntity>(dto);
        if (account.Password.IsNull())
        {
            var config = _configProvider.Get<AdminConfig>();
            account.Password = config.DefaultPassword;
        }

        account.Password = _passwordHandler.Encrypt(account.Password);
        await _repository.Add(account);
        return account.Id;
    }

    public async Task<AccountUpdateDto> Edit(Guid id)
    {
        var account = await _repository.Get(id);

        var model = _mapper.Map<AccountUpdateDto>(account);
        model.Password = "";
        return model;
    }

    public async Task Update(AccountUpdateDto dto)
    {
        var account = await _repository.Get(dto.Id);

        if (dto.Phone.NotNull() && await _repository.ExistsPhone(dto.Phone, dto.Id))
            AdminException.Throw(AdminErrorCode.AccountPhoneExists);

        if (dto.Email.NotNull() && await _repository.ExistsUsername(dto.Email, dto.Id))
            AdminException.Throw(AdminErrorCode.AccountEmailExists);

        if (!await _roleRepository.Exists(dto.RoleId))
            AdminException.Throw(AdminErrorCode.AccountBindRoleExists);

        var username = account.Username;
        var password = account.Password;
        _mapper.Map(dto, account);

        //用户名和密码不允许修改
        account.Username = username;
        account.Password = password;

        await _repository.Update(account);
    }

    public Task Delete(Guid id)
    {
        return _repository.SoftDelete(id);
    }

    public async Task UpdateSkin(AccountSkinUpdateDto dto)
    {
        var config = await _skinRepository.Find(m => m.AccountId == dto.AccountId).ToFirst();

        if (config == null)
        {
            config = new AccountSkinEntity
            {
                AccountId = dto.AccountId
            };
        }

        config.Code = dto.Code;
        config.Name = dto.Name;
        config.Theme = dto.Theme;
        config.Size = dto.Size;

        if (config.Id < 1)
        {
            await _skinRepository.Add(config);
        }

        await _skinRepository.Update(config);
    }

    public Task Activate(Guid id)
    {
        return _repository
            .Find(m => m.Id == id)
            .ToUpdate(m => new AccountEntity
            {
                Status = AccountStatus.Active
            });
    }
}