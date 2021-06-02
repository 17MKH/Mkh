using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Mkh.Auth.Abstractions.Options;
using Mkh.Data.Abstractions.Annotations;
using Mkh.Mod.Admin.Core.Application.Account.Dto;
using Mkh.Mod.Admin.Core.Domain.Account;
using Mkh.Mod.Admin.Core.Domain.AccountRole;
using Mkh.Mod.Admin.Core.Domain.Role;
using Mkh.Mod.Admin.Core.Infrastructure;
using Mkh.Utils.Models;

namespace Mkh.Mod.Admin.Core.Application.Account
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IAccountRepository _repository;
        private readonly IAccountRoleRepository _accountRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IOptionsMonitor<AuthOptions> _authOptions;
        private readonly IPasswordHandler _passwordHandler;

        public AccountService(IMapper mapper, IAccountRepository repository, IOptionsMonitor<AuthOptions> authOptions, IPasswordHandler passwordHandler, IRoleRepository roleRepository, IAccountRoleRepository accountRoleRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _authOptions = authOptions;
            _passwordHandler = passwordHandler;
            _roleRepository = roleRepository;
            _accountRoleRepository = accountRoleRepository;
        }

        [Transaction]
        public async Task<IResultModel> Add(AccountAddDto dto)
        {
            if (await _repository.ExistsUsername(dto.Username))
                return ResultModel.Failed("用户名已存在");
            if (dto.Phone.NotNull() && await _repository.ExistsPhone(dto.Phone))
                return ResultModel.Failed("手机号已存在");
            if (dto.Email.NotNull() && await _repository.ExistsUsername(dto.Email))
                return ResultModel.Failed("邮箱已存在");

            var account = _mapper.Map<AccountEntity>(dto);
            if (account.Password.IsNull())
            {
                account.Password = _authOptions.CurrentValue.DefaultPassword;
            }

            account.Password = _passwordHandler.Encrypt(account.Password);

            if (await _repository.Add(account))
            {
                if (dto.Roles != null && dto.Roles.Any())
                {
                    dto.Roles.ForEach(async r =>
                    {
                        if (await _roleRepository.Find(m => m.Code == r).ToExists())
                        {
                            await _accountRoleRepository.Add(new AccountRoleEntity
                            {
                                AccountId = account.Id,
                                RoleCode = r
                            });
                        }
                    });
                }

                return ResultModel.Success();
            }

            return ResultModel.Failed();
        }
    }
}
