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

public class AccountService : IAccountService
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

    public async Task<IResultModel> Add(AccountAddDto dto)
    {
        if (await _repository.ExistsUsername(dto.Username))
            return ResultModel.Failed("用户名已存在");
        if (dto.Phone.NotNull() && await _repository.ExistsPhone(dto.Phone))
            return ResultModel.Failed("手机号已存在");
        if (dto.Email.NotNull() && await _repository.ExistsUsername(dto.Email))
            return ResultModel.Failed("邮箱已存在");
        if (!await _roleRepository.Exists(dto.RoleId))
            return ResultModel.Failed("绑定角色不存在");

        var account = _mapper.Map<AccountEntity>(dto);
        if (account.Password.IsNull())
        {
            var config = _configProvider.Get<AdminConfig>();
            account.Password = config.DefaultPassword;
        }

        account.Password = _passwordHandler.Encrypt(account.Password);
        var result = await _repository.Add(account);
        return ResultModel.Result(result);
    }

    public async Task<IResultModel> Edit(Guid id)
    {
        var account = await _repository.Get(id);
        if (account == null)
            return ResultModel.NotExists;

        var model = _mapper.Map<AccountUpdateDto>(account);
        model.Password = "";

        return ResultModel.Success(model);
    }

    public async Task<IResultModel> Update(AccountUpdateDto dto)
    {
        var account = await _repository.Get(dto.Id);
        if (account == null)
            return ResultModel.NotExists;
        if (dto.Phone.NotNull() && await _repository.ExistsPhone(dto.Phone, dto.Id))
            return ResultModel.Failed("手机号已存在");
        if (dto.Email.NotNull() && await _repository.ExistsUsername(dto.Email, dto.Id))
            return ResultModel.Failed("邮箱已存在");
        if (!await _roleRepository.Exists(dto.RoleId))
            return ResultModel.Failed("绑定角色不存在");

        var username = account.Username;
        var password = account.Password;
        _mapper.Map(dto, account);

        //用户名和密码不允许修改
        account.Username = username;
        account.Password = password;

        var result = await _repository.Update(account);
        return ResultModel.Result(result);
    }

    public async Task<IResultModel> Delete(Guid id)
    {
        var account = await _repository.Get(id);
        if (account == null)
            return ResultModel.NotExists;

        var result = await _repository.SoftDelete(id);

        return ResultModel.Result(result);
    }

    public async Task<IResultModel> UpdateSkin(AccountSkinUpdateDto dto)
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
            return ResultModel.Result(await _skinRepository.Add(config));
        }

        return ResultModel.Result(await _skinRepository.Update(config));
    }

    public Task<bool> Activate(Guid id)
    {
        return _repository
            .Find(m => m.Id == id)
            .ToUpdate(m => new AccountEntity
            {
                Status = AccountStatus.Active
            });
    }
}