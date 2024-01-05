using System;
using System.Threading.Tasks;
using Mkh.Mod.Admin.Core.Application.Roles.Dto;
using Mkh.Mod.Admin.Core.Application.Roles.Rto;
using Mkh.Mod.Admin.Core.Domain.Roles;

namespace Mkh.Mod.Admin.Core.Application.Roles;

internal class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public Task<RoleDetailsRto> Create(RoleCreateDto dto)
    {
        throw new NotImplementedException();
    }

    public Task Update(RoleUpdateDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<RoleDetailsRto> Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}