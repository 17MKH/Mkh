using AutoMapper;
using Mkh.Mod.Admin.Core.Application.Account.Dto;
using Mkh.Mod.Admin.Core.Domain.Account;
using Mkh.Utils.Mapper;

namespace Mkh.Mod.Admin.Core.Application.Account
{
    public class MapperConfig : IMapperConfig
    {
        public void Bind(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<AccountAddDto, AccountEntity>();
        }
    }
}
